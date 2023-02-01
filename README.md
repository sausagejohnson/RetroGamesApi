# Retro Games Demo API

## Purpose

This is very simple API for doing quick development tests. It does not require any API Keys or authentication.

## Endpoints

Three endpoints are supported:

http://retrogames.waynejohnson.net/api/games GET

This will return a small list of the greatest games ever created.

http://retrogames.waynejohnson.net/api/games/{id} GET

This will return a single record.

http://retrogames.waynejohnson.net/api/games POST

Raw Body format of:
```
{
	"title": "New Game Title",
	"year": 2023
}
```

This will POST a single game record and return the entire collection.

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

Support for PUT.