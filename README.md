# Retro Games Demo API

## Purpose

This is very simple API for doing quick development tests. It does not require any API Keys or authentication.

## Endpoints

Nine endpoints are supported:

GET http://retrogames.waynejohnson.net/api/games
This will return a small list of the greatest games ever created. Returns an OK status of 200.

GET http://retrogames.waynejohnson.net/api/games/{id}
This will return a single record. Returns an OK status of 200. If there is no game to match the supplied id, a 204 No Content is returned.

POST http://retrogames.waynejohnson.net/api/games
This will POST a single game record and return the entire collection. Returns a Created status of 201.

Raw Request Body is formatted as:
```
{
	"title": "New Game Title",
	"year": 1975
}
```

If the game is not found, a 404 Not Found is returned.


PUT http://retrogames.waynejohnson.net/api/games/{id}
This will allow changing a single record. Returns an OK status of 200.

Raw Request Body is formatted as:
```
{
			"title": "Just Monkey",
			"year": 1990
}
```

If the game is not found, a 404 Not Found is returned.

DELETE http://retrogames.waynejohnson.net/api/games/{id}
This will allow deleting a single record. Returns an OK status of 200, or 404 Not Found if the record does not exist.


GET http://localhost:42566/api/platforms
This will return the entire list of the available platforms. Returns an OK status of 200.


GET http://localhost:42566/api/games/{id}/platforms
Get the list of platforms available under a particular game. Returns a formatted response and a status of 200 OK. If an unknown game id is requested, a 204 No Content is returned. 


POST http://localhost:42566/api/games/{id]/platform/{platformId}
Adds the platformId (unrestricted) to the selected game {id} with a status of 200 OK and returns the updated list of games. If the game {id} does not match any records, a status of 204 No Content is returned.


DELETE http://localhost:42566/api/games/{id}/platform/{platformId}
Adds the chosen platformId (unrestricted) from the selected game {id}. A status of 200 OK is returned and also the updated list of games. If the platformId does not match, the no action occurs. If the game {id} does not match any records, a status of 204 No Content is returned.



## Model

The model (record type) is:

```
{
    "game_id": INT
    "title": STRING
    "year": INT
}
```

## Long term persistance

Data files are unique to whatever client session created it. 
All data files are currently purged after five minutes use.

## Upcoming

Testing and hardening. Potentially restrict invalid platform ids, and passed in junk.

## Further Reading

  - https://blog.waynejohnson.net/doku.php/dotnetcore_webapi_to_windows_plesk
  - https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0