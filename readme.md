# Cronos

Cronos is a webapp that helps the user create a chronological playlist based on an artist in Spotify.

A working version is running at https://cronos.frenetik.io

## Getting Started

This is a standard asp.net core application.  There is an included DOCKERFILE.  To run, you can either debug in Visual Studio, via the command line, or via a docker container.

### Prerequisites

Visual Studio 2017 or a text editor
ASP.NET CORE SDK 2.0+

### Installing

Open the cronos.csproj file.  You need 2 configuration settings to get this working:  A Spotify client id and a spotify secret.  These are coded as:
- SPOTIFY_CLIENTID
- SPOTIFY_CLIENT_SECRET

You can get these by signing up at developer.spotify.com

## Running the tests

No tests yet, sadly.


## Deployment

Add additional notes about how to deploy this on a live system

## Built With

* [.NET Core](https://github.com/aspnet/Home) - The web framework used
* [FluentSpotifyApi](https://github.com/dotnetfan/FluentSpotifyApi) - Great library to interact with Spotify 

## Authors

* **Mike Ruhl** - *Initial work* - [Frenetik](https://frenetik.io)


## License

This project is licensed under the Apache License - see the license file for details

## Acknowledgments

* [dotnetfan](https://github.com/dotnetfan) made my life way easier.  Shout out to them and their great library.
