﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saver.IdentityService.Models;
using Saver.IdentityService.Services;

namespace Saver.IdentityService.Api;

public static class IdentityApi
{
    private static readonly EmailAddressAttribute EmailAttribute = new();

    public static IEndpointRouteBuilder MapIdentityApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/identity");

        api.MapPost("/register", RegisterAsync)
            .AllowAnonymous();
        api.MapPut("/confirm", ConfirmEmail)
            .AllowAnonymous();
        api.MapPut("/edit/email", ChangeEmailAsync);
        api.MapPut("/edit/password", ChangePasswordAsync);
        api.MapDelete("/", DeleteAccountAsync);
        api.MapPost("/login", LoginAsync)
            .AllowAnonymous();

        api.RequireAuthorization();

        return builder;
    }

    private static async Task<Results<Created, ValidationProblem>> RegisterAsync(
        RegistrationModel model, [AsParameters] IdentityServices services)
    {
        if (!EmailAttribute.IsValid(model.Email))
        {
            return CreateValidationProblem("InvalidEmail", $"Value {model.Email} is invalid for an e-mail.");
        }

        var result = await services.AccountService.CreateAccountAsync(model);
        return result.Succeeded ? TypedResults.Created() : CreateValidationProblem(result);
    }

    private static void ConfirmEmail()
    {
        // @TODO: implement e-mail confirmation
    }

    private static async Task<Results<NoContent, ValidationProblem>> ChangeEmailAsync(
        [FromBody] ChangeEmailModel model, [AsParameters] IdentityServices services)
    {
        if (!EmailAttribute.IsValid(model.NewEmail))
        {
            return CreateValidationProblem("InvalidEmail", $"Value {model.NewEmail} is invalid for an e-mail.");
        }

        var userId = services.UserContextProvider.GetUserId();
        if (userId is null)
        {
            return TypedResults.NoContent();
        }

        var result = await services.AccountService.ChangeEmailAsync(userId, model);
        return result is UserNotFoundIdentityResult or { Succeeded: true }
            ? TypedResults.NoContent()
            : CreateValidationProblem(result);
    }

    private static async Task<Results<NoContent, ValidationProblem>> ChangePasswordAsync(
        [FromBody] ChangePasswordModel model, [AsParameters] IdentityServices services)
    {
        var userId = services.UserContextProvider.GetUserId();
        if (userId is null)
        {
            return TypedResults.NoContent();
        }

        var result = await services.AccountService.ChangePasswordAsync(userId, model);
        return result is UserNotFoundIdentityResult or { Succeeded: true }
            ? TypedResults.NoContent()
            : CreateValidationProblem(result);
    }

    private static async Task<Results<NoContent, ValidationProblem>> DeleteAccountAsync(
        [AsParameters] IdentityServices services)
    {
        var userId = services.UserContextProvider.GetUserId();
        if (userId is null)
        {
            return TypedResults.NoContent();
        }

        var result = await services.AccountService.DeleteAccountAsync(userId);
        return result is UserNotFoundIdentityResult or { Succeeded: true }
            ? TypedResults.NoContent()
            : CreateValidationProblem(result);
    }

    private static async Task<Results<Ok<string>, UnauthorizedHttpResult>> LoginAsync(
        LoginModel model, [FromServices] ILoginService loginService)
    {
        var result = await loginService.LoginAsync(model);
        return result is LoggedInIdentityResult loggedInResult
            ? TypedResults.Ok(loggedInResult.Token)
            : TypedResults.Unauthorized();
    }

    private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription)
    {
        return TypedResults.ValidationProblem(new Dictionary<string, string[]>
        {
            [errorCode] = [errorDescription]
        });
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        var errorCodeToDescriptions = new Dictionary<string, string[]>(1);
        foreach (var identityError in result.Errors)
        {
            string[] newDescriptions;

            if (errorCodeToDescriptions.TryGetValue(identityError.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = identityError.Description;
            }
            else
            {
                newDescriptions = [identityError.Description];
            }

            errorCodeToDescriptions[identityError.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorCodeToDescriptions);
    }
}