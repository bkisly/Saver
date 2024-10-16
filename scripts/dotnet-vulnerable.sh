#!/bin/bash
echo "Beginning vulnerable packages check..."
dotnet list package --vulnerable 2>&1 | tee ./build.log
echo "Analyzing log file for vulnerabilities..."
grep -q -i "critical\|high" ./build.log
rc=$?

if [ $rc -eq 0 ]; then
  exit 1
elif [ $rc -eq 1 ]; then
  exit 0
else
  exit $rc
fi
