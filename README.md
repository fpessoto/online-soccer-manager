** Write a soccer online manager game API **

<p>You need to write a RESTful or GraphQL API for a simple application where football/soccer fans will create fantasy teams and will be able to sell or buy players.</p>
<p>User must be able to create an account and log in using the API. </p>
<p>Each user can have only one team (user is identified by an email)</p>
<p>When the user is signed up, they should get a team of 20 players (the system should generate players):
  <li>3 goalkeepers</li>
  <li>6 defenders</li>
  <li>6 midfielders</li>
  <li>5 attackers</li>
 </p>
<p>Each player has an initial value of $1.000.000.</p>
<p>Each team has an additional $5.000.000 to buy other players.</p>
<p>When logged in, a user can see their team and player information</p>
<p>Team has the following information:
  <li>Team name and a country (can be edited)</li>
  <li>Team value (sum of player values)</li>
</p>
<p>Player has the following information
  <li>First name, last name, country (can be edited by a team owner)</li>
  <li>Age (random number from 18 to 40) and market value </li>
</p>
<p>A team owner can set the player on a transfer list</p>
<p>When a user places a player on a transfer list, they must set the asking price/value for this player. This value should be listed on a market list. When another user/team buys this player, they must be bought for this price. </p>
<p>Each user should be able to see all players on a transfer list.</p>
<p>With each transfer, team budgets are updated.</p>
<p>When a player is transferred to another team, their value should be increased between 10 and 100 percent. Implement a random factor for this purpose.</p>
<p>Make it possible to perform all user actions via RESTful or GraphQL API, including authentication.</p>
