# Retro Games API

## Purpose

This is very simple API for doing quick development tests. It does not require any API Keys or authentication.

## Endpoints

Two endpoints are supported:

http://retrogames.waynejohnson.net/api/games GET

This will return a small list of the greatest games ever created.

http://retrogames.waynejohnson.net/api/games/{id} GET

This will return a single record.

## Model

The model (record type) is:

```
{
    "game_id": INT
    "title": STRING
    "year": INT
}
```