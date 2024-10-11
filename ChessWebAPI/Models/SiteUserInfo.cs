using ChessGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ChessGameClient.Models
{
    public class SiteUserInfo : IChessObservable
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public List<string> roles = new List<string>();
        public List<string> Roles
        {
            get => roles;
            set
            {
                roles = value;
                ((IChessObservable)this).Notify();
            }
        }
        private bool status = false;
        private bool rivalStatus = false;
        public bool Status { get => status; set { status = value; ((IChessObservable)this).Notify(); } }
        public bool RivalStatus { get => rivalStatus; set { rivalStatus = value; ((IChessObservable)this).Notify(); } }
        public int RivalId { get; set; }
        public string RivalName { get; set; }
        public DateTime AccessTokenExpire { get; set; } = DateTime.UtcNow + TimeSpan.FromMinutes(5);
        public List<IChessObserver> Observers { get; set; } = new List<IChessObserver>();
        public void Update(IEnumerable<Claim> claims)
        {
            Username = claims.First(c => c.Type.Equals("given_name")).Value;
            Name = claims.First(c => c.Type.Equals("email")).Value;
            Id = int.Parse(claims.First(c => c.Type.Equals("nameid")).Value);
            Roles = claims
                .Where(claim => claim.Type.Equals("role"))
                .Select(c => c.Value)
                .ToList();
            var time = long.Parse(claims.First(c => c.Type.Equals("exp")).Value);
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);
            AccessTokenExpire = dateTimeOffset.UtcDateTime;
        }
        public void Default()
        {
            Name = "Guest";
            Id = 0;
            Roles = new List<string>();
        }
    }
}
