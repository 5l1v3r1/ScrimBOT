using Discord;
using Discord.WebSocket;
using MyCustomDiscordBot.Extensions;
using MyCustomDiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MyCustomDiscordBot.Services
{
    public class EmbedService
    {
        private readonly DiscordSocketClient _client;

        private readonly DatabaseService _databaseService;

        private Random random = new Random();

        public EmbedService(DiscordSocketClient client, DatabaseService databaseService)
        {
            _client = client;
            _databaseService = databaseService;
        }

        public async Task<Embed> ScrimBoardEmbed(List<ScrimInvite> invites)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Green);
            string description = "**USER POSTING | TEAM POINTS | SCRIM DESCRIPTION**\n";
            foreach (ScrimInvite invite in invites)
            {
                description += $"\n{invite.InvitingTeamCaptain.Mention} | {invite.InvitingTeam.Points} | `{invite.Description}`";
            }
            builder.WithDescription(description);
            builder.WithFooter(new EmbedFooterBuilder
            {
                Text = "\nUse .post scrimDescription --> to make a post.\nUse .removepost --> to remove your team's post.\nUse .accept @PostingUser to accept their challenge."
            });
            return builder.Build();
        }
        public string[] Getrank(int elo)
        {

            if (elo <= 0)
            {
                string[] level = { "<:norank050:909110612456517632>", "<:pronz50100:909110618752155699>", "50" , "No Rank <:norank050:909110612456517632>" };

                return level;

            }
            if (elo <= 50)
            {

                string[] level = { "<:pronz50100:909110618752155699>", "<:silver100200:909110626134163477>", "180", "Bronze <:pronz50100:909110618752155699>" };

                return level;
            }
            if (elo <= 180)
            {
                string[] level = { "<:silver100200:909110626134163477>", "<:gold200300:909110626742337546>", "250", "Silver <:silver100200:909110626134163477>" };

                return level;

            }
            if (elo <= 250)
            {
                string[] level = { "<:gold200300:909110626742337546>", "<:plat300400:909110628818501702>", "350", "Gold <:gold200300:909110626742337546>" };

                return level;

            }
            if (elo <= 350)
            {

                string[] level = { "<:plat300400:909110628818501702>", "<:diamond400500:909110629766418442>", "500", "Platinum <:plat300400:909110628818501702>" };

                return level;
            }
            if (elo <= 500)
            {

                string[] level = { "<:diamond400500:909110629766418442>", "<:Master500600:909110629795758130>", "750", "Diamond <:diamond400500:909110629766418442>" };

                return level;
            }
            if (elo <= 750)
            {
                string[] level = { "<:Master500600:909110629795758130>", "<:legend600700:909110629288263722>", "850", "Master <:Master500600:909110629795758130>" };

                return level;

            }
            if (elo <= 850)
            {
                string[] level = { "<:legend600700:909110629288263722>", "<:mythical700900:909109955955654686>", "1000", "Mythical <:mythical700900:909109955955654686>" };

                return level;

            }
            if (elo <= 1000)
            {
                string[] level = { "<:mythical700900:909109955955654686>", "<:grandm9001000:909109348633026590>", "1000" , "Grand Master <:grandm9001000:909109348633026590>" };

                return level;

            }

            string[] levelp = { "<:grandm9001000:909109348633026590>", "<:grandm9001000:909109348633026590>","99999", "Grand Master <:grandm9001000:909109348633026590>" };
            return levelp;
        }
        public async Task<Embed> LeaderboardEmbed(List<DbUser> topPlayers)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("**RANK | ELO | PLAYER**");
            builder.WithColor(Color.Red);
            string description = "";

            for (int i = 0; i < topPlayers.Count; i++)
            {
                SocketUser user = _client.GetUser(topPlayers[i].DiscordId);
                if (user != null)
                {
                    description += $"`#{i + 1} | {topPlayers[i].ELO}` {user.Mention}\n{Getrank(topPlayers[i].ELO)[0]}   {topPlayers[i].ELO.ToDiscordProgressBar(12)} {Getrank(topPlayers[i].ELO)[1]}    `{topPlayers[i].ELO}/{Getrank(topPlayers[i].ELO)[2]}`\n";
                }
            }
            builder.WithDescription(description);
            builder.WithFooter(new EmbedFooterBuilder
            {
                Text = "Leaderboard"
            });
            return builder.Build();
        }

        public async Task<Embed> TeamEmbed(Team team)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Blue);
            builder.WithTitle(team.Name + "'s Stats");
            int totalWins = 0;
            int totalLosses = 0;
            foreach (MapRecord record in team.MapRecords)
            {
                builder.AddField(record.MapName, $"{record.Wins} | {record.Losses}", inline: true);
                totalWins += record.Wins;
                totalLosses += record.Losses;
            }
            int gamesPlayed = totalLosses + totalWins;
            string memberMentions = "\n";
            foreach (ulong discordId in team.MemberDiscordIds)
            {
                if (discordId != team.CaptainDiscordId)
                {
                    SocketUser user = _client.GetUser(discordId);
                    if (user != null)
                    {
                        memberMentions = memberMentions + user.Mention + "\n";
                    }
                }
            }
            memberMentions += "\n";
            builder.WithDescription($"**Captain:** {_client.GetUser(team.CaptainDiscordId).Mention}\n**Members**: {memberMentions} **Points**: `{team.Points}`\n**Games Played**: `{gamesPlayed}` | Wins: `{totalWins}` Losses `{totalLosses}`\n\n");
            return builder.Build();
        }
        public async Task<Embed> Pic(ulong user)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Blue);
            builder.WithTitle("Avatar");
            builder.WithImageUrl(_client.GetUser(user).GetAvatarUrl(ImageFormat.Auto, 128));
            builder.WithFooter($"{_client.GetUser(user).Username}");

            return builder.Build();
        }
        public Embed ProfileEmbed(DbUser user)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Blue);
            builder.WithTitle(user.Username + "'s Stats");
            int totalWins = 0;
            int totalLosses = 0;
            foreach (MapRecord record in user.MapRecords)
            {
                builder.AddField(record.MapName, $"{record.Wins} | {record.Losses}", inline: true);
                totalWins += record.Wins;
                totalLosses += record.Losses;
            }
            int gamesPlayed = totalLosses + totalWins;
            var elo = $"\n{Getrank(user.ELO)[0]} {user.ELO.ToDiscordProgressBar(12)} {Getrank(user.ELO)[1]} `{user.ELO}/{Getrank(user.ELO)[2]}`";
            builder.WithDescription($"**ELO**: `{user.ELO}`\n**RANK**: {Getrank(user.ELO)[3]}\n**Games Played**: `{gamesPlayed}` | Wins: `{totalWins}` Losses `{totalLosses}`\n{elo}");
            builder.WithThumbnailUrl(_client.GetUser(user.DiscordId).GetAvatarUrl(ImageFormat.Auto, 128));
            return builder.Build();
        }

        public async Task<Embed> MatchLogEmbed(Match match, ulong guildId)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Red);
            builder.WithTitle($"Match Log: #{match.Number}");
            if (match.Winners > 0)
            {
                builder.WithFooter(new EmbedFooterBuilder
                {
                    Text = $"Team {match.Winners} victory!"
                });
            }
            else if (match.Winners == -2)
            {
                builder.WithFooter(new EmbedFooterBuilder
                {
                    Text = "Decision reversed! No elo granted."
                });
            }
            else if (match.Winners == 0)
            {
                builder.WithFooter(new EmbedFooterBuilder
                {
                    Text = "Match cancelled!"
                });
            }
            builder.WithDescription("Map: *" + match.Map + "*");
            string team1Value = "";
            foreach (ulong discordId2 in match.Team1DiscordIds)
            {
                team1Value += $"`{(await _databaseService.GetUserInGuild(discordId2, guildId)).ELO}`{_client.GetUser(discordId2).Mention}\n";
            }
            string team2Value = "";
            foreach (ulong discordId2 in match.Team2DiscordIds)
            {
                team2Value += $"`{(await _databaseService.GetUserInGuild(discordId2, guildId)).ELO}`{_client.GetUser(discordId2).Mention}\n";
            }
            builder.AddField("Team 1", team1Value, inline: true);
            builder.AddField("Team 2", team2Value, inline: true);
            return builder.Build();
        }

        public Embed GetPugQueueCreatedEmbed(QueueConfig config)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"{config.Name} Queue: 0 / {config.Capacity}");
            builder.WithDescription("Click the **Join button** to join the Mixed EG/EU queue or Click **leave button** to leave queue\n\n**Players**\n");
            builder.WithColor(Color.Gold);

            builder.WithFooter(new EmbedFooterBuilder
            {
                Text = $"Powered by CrossFire Stars League"
            });
            return builder.Build();
        }

        public Embed GetQueueEmbed(PugQueue queue)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Gold);
            builder.WithTitle($"{queue.Name}");
            string description = "";
            foreach (DbUser user in queue.Users)
            {
                description += $"`{user.ELO}`{_client.GetUser(user.DiscordId).Mention}\n";
            }
            builder.WithDescription($"Click the **Join button** to join the Mixed EG/EU queue or Click **leave button** to leave queue\n\n**Players**                       **{queue.Users.Count()} / {queue.Capacity}**\n" + description);
            builder.WithFooter(new EmbedFooterBuilder
            {
                Text = $"Powered by CrossFire Stars League"
            });
            return builder.Build();
        }

        public string RandomString(int length)
        {
            return new string((from s in Enumerable.Repeat("123364654654564974548548484", length)
                               select s[random.Next(s.Length)]).ToArray());
        }

        public async Task<Embed> GetMatchEmbedAsync(Match match, ulong guildId, Team team1 = null, Team team2 = null)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithColor(Color.Green);
            string password = string.Empty;

            if (guildId == ServerIDs())
            {

                password = RandomString(3).ToLower();


            }
            if (match.SortType == SortType.Elo)
            {
                builder.WithTitle($"PUG Match #{match.Number}");
                if (password != string.Empty)
                {
                    builder.WithDescription("Map: *" + match.Map + "*\n\nPassword: *" + password + "*");

                }
                else
                {
                    builder.WithDescription("Map: *" + match.Map + "*");
                }
                string team1Value3 = "";
                foreach (ulong discordId5 in match.Team1DiscordIds)
                {
                    team1Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                }
                string team2Value3 = "";
                foreach (ulong discordId5 in match.Team2DiscordIds)
                {
                    team2Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                }

                builder.AddField("Team 1", team1Value3, inline: true);
                builder.AddField("Team 2", team2Value3, inline: true);



                string Mapup = match.Map.ToUpper();
                //qConfig.Maps.Add("SubBase");
                //qConfig.Maps.Add("ankara");
                //qConfig.Maps.Add("BlackWidow");
                //qConfig.Maps.Add("Compound");
                //qConfig.Maps.Add("Port");
                if (Mapup.Contains("SUBBASE"))

                {

                    builder.WithImageUrl("https://cdn.discordapp.com/attachments/691520066575138866/906938436341104651/1000.png");



                }
                if (Mapup.Contains("COMPOUND"))
                {




                    builder.WithImageUrl("https://cdn.discordapp.com/attachments/691520066575138866/906938077711331338/latest.jpg");



                }
                if (Mapup.Contains("PORT"))
                {




                    builder.WithImageUrl("https://media.discordapp.net/attachments/691520066575138866/906939312652832828/480.png");



                }
                if (Mapup.Contains("BLACKWIDOW"))
                {




                    builder.WithImageUrl("https://media.discordapp.net/attachments/691520066575138866/906939006724481054/1000.png?width=901&height=676");



                }
                if (Mapup.Contains("ANKARA"))
                {




                    builder.WithImageUrl("https://media.discordapp.net/attachments/691520066575138866/906938745842970644/1000.png");



                }

            }
            else if (match.SortType == SortType.Captains)
            {
                builder.WithTitle($"PUG Match #{match.Number}");
                if (match.PickingPoolDiscordIds.Count > 0)
                {
                    List<string> poolMentions = new List<string>();
                    foreach (ulong discordId8 in match.PickingPoolDiscordIds)
                    {
                        poolMentions.Add(_client.GetUser(discordId8).Mention);
                    }
                    string description = "Map: *" + match.Map + "*\n\n" + _client.GetUser(match.ActiveCaptainDiscordId).Mention + " is picking... please pick from a user in this pool: " + string.Join(" ", poolMentions);
                    builder.WithDescription(description + "\n\n*Pick using `.pick @user`*");
                    string team2Value3 = "";
                    foreach (ulong discordId5 in match.Team1DiscordIds)
                    {
                        team2Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                    }
                    string team1Value3 = "";
                    foreach (ulong discordId5 in match.Team2DiscordIds)
                    {
                        team1Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                    }
                    builder.AddField("Team 1", team2Value3, inline: true);
                    builder.AddField("Team 2", team1Value3, inline: true);
                }
                else
                {
                    if (password != string.Empty)
                    {
                        builder.WithDescription("Map: *" + match.Map + "*\n\nPassword: *" + password + "*");
                    }
                    else
                    {
                        builder.WithDescription("Map: *" + match.Map + "*");
                    }
                    string team1Value3 = "";
                    foreach (ulong discordId5 in match.Team1DiscordIds)
                    {
                        team1Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                    }
                    string team2Value3 = "";
                    foreach (ulong discordId5 in match.Team2DiscordIds)
                    {
                        team2Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                    }
                    builder.AddField("Team 1", team1Value3, inline: true);
                    builder.AddField("Team 2", team2Value3, inline: true);
                }
            }
            else if (match.SortType == SortType.Scrim)
            {
                builder.WithTitle($"Scrim Match #{match.Number}");
                builder.WithDescription("*Please choose and agree upon any map and game mode.*");
                string team2Value3 = "";
                foreach (ulong discordId5 in match.Team1DiscordIds)
                {
                    team2Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                }
                string team1Value3 = "";
                foreach (ulong discordId5 in match.Team2DiscordIds)
                {
                    team1Value3 += $"`{(await _databaseService.GetUserInGuild(discordId5, guildId)).ELO}`{_client.GetUser(discordId5).Mention}\n";
                }
                builder.AddField($"1: {team1.Name} [{team1.Points}]", team2Value3, inline: true);
                builder.AddField($"2: {team2.Name} [{team2.Points}]", team1Value3, inline: true);
            }
            builder.WithFooter(new EmbedFooterBuilder
            {
                Text = "Powered by " + _client.GetGuild(guildId).Name
            });
            return builder.Build();
        }

        public static ulong ServerIDs() => Config.id;
    }
}
