﻿using GOGDotNet.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GOGDotNet
{
    public class GOGClient
    {
        private readonly RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="GOGClient"/> class.
        /// </summary>
        /// <param name="baseUrl">Optional. Specifies the base URL for the GOG.com website.</param>
        public GOGClient(string baseUrl = "https://www.gog.com/")
        {
            this.client = new RestClient(baseUrl);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public virtual async Task<(ProfileState, IEnumerable<Game>)> GetGamesStatsAsync(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                throw new ArgumentException($"Argument '{nameof(userID)}' must not be null or empty.");
            }

            // Query GOG for the list of games in the given user's library
            // GOG returns a fixed number of items per 'page', but specifies the total number of pages in each response
            List<Game> games = new List<Game>();
            int numPages = 1;
            for (int i = 0; i < numPages; i++)
            {
                // Get the response back for the current page
                (ProfileState profileState, GogGameResponse responseObject) = await this.GetResponsePageAsync(userID, i + 1);
                if (profileState != ProfileState.Verified || responseObject == null)
                {
                    // If we failed to retrieve a response, return null immediately
                    return (profileState, null);
                }

                // On the first response, check for the number of pages that we'll need to retrieve
                if (i == 0)
                {
                    numPages = responseObject.pages;
                }

                // Convert response items to Game objects and add to list
                List<GogItem> responseItemList = new List<GogItem>(responseObject._embedded.items);
                games.AddRange(responseItemList.Select(responseItem =>
                {
                    string imageUrl = responseItem?.game?.image;
                    if (imageUrl != null)
                    {
                        // Update image URL to get full-size image
                        imageUrl = imageUrl.Replace(".png", "_prof_game_200x120.png");
                    }


                    var game = new Game()
                    {
                        AchievementSupport = responseItem?.game?.achievementSupport ?? false,
                        Id = ulong.Parse(responseItem?.game?.id),
                        Image = imageUrl,
                        Title = responseItem?.game?.title,
                        Url = responseItem?.game?.url
                    };

                    if (responseItem?.stats != null)
                    {
                        var stats = responseItem.stats.First().Value;
                        game.LastSession = stats.lastSession;
                        game.AchievementsPercentage = stats.achievementsPercentage;
                        game.Playtime = stats.playtime;
                    }

                    return game;
                }));
            }

            // Return complete list of Game objects
            return (ProfileState.Verified, games);
        }

        private async Task<(ProfileState, GogGameResponse)> GetResponsePageAsync(string gogId, int pageNumber)
        {
            var request = new RestRequest(
                string.Format(CultureInfo.InvariantCulture, "/u/{0}/games/stats?page={1}", gogId, pageNumber));

            var response = await this.client.ExecuteGetAsync<GogGameResponse>(request);

            if (response == null || response.Data == null)
            {
                // Null response means the request failed
                return (ProfileState.VerificationFailed, null);
            }
            else if (!response.IsSuccessful)
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Forbidden:
                        // Forbidden response indicates that profile exists but is private
                        return (ProfileState.Private, null);

                    case System.Net.HttpStatusCode.NotFound:
                        // NotFound response indicates that profile does not exist
                        return (ProfileState.DoesNotExist, null);

                    default:
                        // Other non-success status codes are just interpreted as failure
                        return (ProfileState.VerificationFailed, null);
                }
            }

            return (ProfileState.Verified, response.Data);
        }
    }
}
