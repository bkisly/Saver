
## Functional requirements

- **Register users** to create their own spaces for managing their finances
	- login with e-mail, set the password and the default currency
	- e-mails must be verified
	- accounts can be deleted
	- (**optional**) provide a way to sign with OAuth2.0
	
- Users **create bank accounts** to represent their real bank accounts (or budgets)
	- a user can have multiple bank accounts
	- a bank account has the initial balance (can be changed later)
	- bank account has assigned a currency
	- if a currency needs to be changed, all registered finances are recalculated accordingly to the current exchange rate
	- bank accounts can be named (names must be unique)
	- users can track balance for their bank accounts as they register their finances
	- user can set 1 account as the main account
	- Accounts can be set as:
		- manual - users define all transactions "by hand"
		- (**optional**) external - automatically linked to their bank accounts or providers, e.x. PayPal. Those accounts have 1-1 relationships with their real accounts, although users can edit all transactions info except value and date

- Users can **register their transactions** for manual accounts
	- Users specify a name, date, optional description, transaction value, optional category and optional priority (defined as 1-5 stars)
	- There are several predefined categories for the most common transaction types, but users can define their own categories (unique names, personalize with colors)
	- To reduce the amount of manual work to be done by users, there are 3 ways to automate  this process:
		1. Define periodic transactions, which are added manually accordingly to various cycle options
		2. Provide AI-assistant section, which can be prompted to describe some last transactions with their values. The assistant proposes the group of transactions which were created based on the given description. User can review them, edit if necessary and add all at once.
		3. Provide AI-powered image analysis tool, which can read transaction info from an invoice or receipt image and prepare transaction for the app
	- Users in general perform regular CRUD operations on transactions

- Users must have a **quick way to display the most important information** regarding their finances. This covers:
	- balance for their main account (and balance change in last 7 days)
	- a small graph showing balance changes for the last 30 days
	- top 3 benefits and expenses categories for the last 30 days
	- budget metrics for the main account
	- quick tip from AI-assistant regarding current status of the budget

- Users should be able to **analyze their finances in more detail**:
	- display a graph for:
		- balance
		- expenses (optionally filtered by categories)
		- benefits (optionally filtered by categories)
	- analysis is limited to the selected time period and account
	- display the list of categories for benefits and expenses sorted by total value for the selected time period (ascending or descending)
	- display the general trend for the selected period as a straight line on the graph
	- for the balance, as well as categories list, display the difference between current and previous time period

- (**optional**) Users can specify **budgets** for their accounts and **track the future status of their finances**:
	- for the selected period of time, budget can be specified as
		- critical balance that cannot be reduced
		- minimal amount of expected income
	- budget can be defined as one-time or automatically renewed
	- users can access metrics about the status of the budget
		- metrics come from analysed condition of the budget at the end of the selected period (ML-powered analysis)
		- budget condition can be rated as endangered (if the prediction is way below the expected values), correct (if it's around the expected values) or stable (if it's highly above the expected values)
		- budget info show defined and predicted values at the end of the covered period
	- display a graph for predicted future changes of account balance for the selected time period

- Display **AI-powered tips and tricks** regarding enhancing saving skills, both general and based on user's finances

## Non-functional requirements

- **Security** - users can store potentially vulnerable data, so it must be well-protected against any threats
- **Resiliency** - the system must be resilient to any issues coming from any infrastructural or internal problems and thus, be always available to the users
- **Scalability** - the final amount of expected features, as well as potential users is unknown. Therefore, the system must properly handle the increased load in certain areas
- **Availability** - the system must be always available to the users and handle internal problems properly
- **Cross-platform support** - the system should be available on various platforms for all users, via web app and optionally mobile apps for Android and iOS
