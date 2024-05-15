# Project Name

## Setup

1. Clone this repository to your local machine.
2. Navigate to the project directory.
3. Dependencies: 
    1. *Swashbuckle.AspNetCore*
    2. *Serilog*
    3. *Serilog.AspNetCore*


## Reddit Account and OAuth Setup

1. Go to [reddit.com](https://www.reddit.com) and create an account. You will need a username and password.
2. Follow the prompts in the *Authorization* section of the OAuth API wiki at [Reddit OAuth2](https://github.com/reddit-archive/reddit/wiki/OAuth2) to set up OAuth for your account.
3. Log into Reddit, navigate to the *App Preferences* under `user settings`, and create a new application. This involves choosing an application type (which should be `script`), setting a name (I used `TrafficAPI`), description, and redirect URI.
4. Take the `clientId` under the app name and `secret` and add them to your user secrets.
5. Store your *Username* and *Password* for your reddit account in your `secrets.json` file (user secrets) as well.
6. In the source code, modify the `REDIRECT_URI` in the `RedditOAuthService.cs` file to match the one in the app you created. 


## Running the Project

1. Restore all Nuget Packages
2. Build the Project
3. Run the Project.