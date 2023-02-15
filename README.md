# Retro Games Demo API

## Purpose

This is very simple API for doing quick development tests. It does not require any API Keys or authentication.

## Endpoints

Three endpoints are supported:

http://retrogames.waynejohnson.net/api/games GET
This will return a small list of the greatest games ever created. Returns an OK status of 200.

http://retrogames.waynejohnson.net/api/games/{id} GET
This will return a single record. Returns an OK status of 200.

http://retrogames.waynejohnson.net/api/games POST

Raw Body format of:
```
{
	"title": "New Game Title",
	"year": 2023
}
```

This will POST a single game record and return the entire collection. Returns a Created status of 201.

http://retrogames.waynejohnson.net/api/games/{id} PUT
This will allow changing a single record. Returns an OK status of 200.

http://retrogames.waynejohnson.net/api/games/{id} DELETE
This will allow deleting a single record. Returns an OK status of 200, or 404 if the record cannot be found.

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

Testing and hardening.

## Further Reading

  - https://blog.waynejohnson.net/doku.php/dotnetcore_webapi_to_windows_plesk
  - https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0