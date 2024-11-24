﻿using Resunet.DAL.Models;
using Resunet.DAL;
using Resunet.BL.General;

namespace Resunet.BL.Auth
{
    public class DbSession : IDbSession
    {
        private readonly IDbSessionDAL sessionDAL;
        private readonly IWebCookie webCookie;

        public DbSession(IDbSessionDAL sessionDAL, IWebCookie webCookie)
        {
            this.sessionDAL = sessionDAL;
            this.webCookie = webCookie;
        }

        private void CreateSessionCookie(Guid sessionid)
        {
            this.webCookie.Delete(AuthConstants.SessionCookieName);
            this.webCookie.AddSecure(AuthConstants.SessionCookieName, sessionid.ToString());
        }

        private async Task<SessionModel> CreateSession()
        {
            var data = new SessionModel()
            {
                DbSessionId = Guid.NewGuid(),
                Created = DateTime.Now,
                LastAccessed = DateTime.Now
            };

            await sessionDAL.Create(data);
            return data;
        }

        private SessionModel? sessionModel = null;
        public async Task<SessionModel> GetSession()
        {
            if (sessionModel != null)
                return sessionModel;

            Guid sessionId;
            var sessionString = webCookie.Get(AuthConstants.SessionCookieName);
            if (sessionString != null)
                sessionId = Guid.Parse(sessionString);
            else
                sessionId = Guid.NewGuid();

            var data = await this.sessionDAL.Get(sessionId);
            if (data == null)
            {
                data = await this.CreateSession();
                CreateSessionCookie(data.DbSessionId);
            }
            sessionModel = data;
            return data;
        }

        public async Task<int> SetUserId(int userId)
        {
            var data = await this.GetSession(); // ожидается сессия
            data.UserId = userId;
            data.DbSessionId = Guid.NewGuid(); // новая сессия
            CreateSessionCookie(data.DbSessionId);
            return await sessionDAL.Create(data);
        }

        public async Task<int?> GetUserId()
        {
            var data = await this.GetSession();
            return data.UserId;
        }

        public async Task<bool> IsLoggedIn()
        {
            var data = await this.GetSession();
            return data.UserId != null;
        }

        public async Task Lock()
        {
            var data = await this.GetSession();
            await sessionDAL.Lock(data.DbSessionId);
        }

        // хелпер, только для тестов, чтобы пошел и проверил в бд
        public void ResetSessionCache()
        {
            sessionModel = null;
        }
    }
}