# Some Thoughts on the Exercise

## Appsettings
- Appsettings need to have an access token in, or pass as an environment variable and change the uncommented code in `DataService`.

## Error Handling
- Programs run happy flow; in production code, I would probably lean towards global error handling for all types of exceptions, depending of course on the scale of the project.

## Access Token
- In this case, only the secret is stored in the config file. Ideally, all secrets would be passed in during the deployment as environment variables, all depending on the deployment flow and technology used. `DataService` has a piece of code commented out for this case.

## Testing

### Unit Testing
- This exercise has little logic into it, but if I were testing this mediator approach, the one unit test there would be a template for it. Testing the handler with mocked requests and inspecting services inside as well as the generated outcome.

### Integration Testing
- The idea here is to have black box testing where I call the endpoints in the application and compare results as plain JSON. There, I would mock any data source, in this case, the HTTP client to fetch close-to-real data from mock storage (I use JSON there, but a DB provider would take it from an in-memory or dockerized populated database) and compare results as `JObjects`, parsed models, or ideally plain JSONs as it is the actual response from the endpoint. Similar with the incoming requests, I would have JSON that would mimic what would normally come in an HTTP request from the Front End.


# Extra exercise:

## Dropdown for Selecting League (Fetching Plain League Data from API)
Implement a dropdown menu to list all available leagues, populated dynamically by making an API call to fetch the leagues. When a league is selected, make an API call to fetch the unbranded upcoming matches for that league and display these matches in a section of the UI, initially without any branding.

## Dropdown for Selecting Brand
Create a second dropdown for brand selection, listing the available brands. This dropdown should not fetch or display any data by itself, but will be used in conjunction with the league selection for fetching branded data.

## Fetching and Displaying Branded Upcoming Matches
Add an event listener to both dropdowns such that when either a league or a brand is selected (or changed), an API call is made with the selected league and brand as parameters. The API responds with the list of upcoming matches for the selected league, branded according to the selected brand. Update the matches display section with this branded data, which may include branding-specific elements like colors, logos, or layouts.

## Interactivity and Responsiveness
Ensure the interface responds immediately to user selections, updating the match listings with the appropriate branding. Make the mockup responsive to accommodate different screen sizes.

## Feedback and Error Handling
Provide visual feedback during data loading, such as a spinner or loading message. Handle errors gracefully, such as API call failures or situations where no matches are found for a selected league.
