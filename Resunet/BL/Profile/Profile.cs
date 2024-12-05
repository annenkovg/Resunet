﻿using Resunet.DAL;
using Resunet.DAL.Models;

namespace Resunet.BL.Profile
{
    public class Profile : IProfile
    {
        private readonly IProfileDAL profileDAL;
        private readonly ISkillDAL skillDAL;

        public Profile(IProfileDAL profileDAL, ISkillDAL skillDAL)
        {
            this.profileDAL = profileDAL;
            this.skillDAL = skillDAL;
        }

        public async Task AddOrUpdate(ProfileModel model)
        {
            if (model.ProfileId == null)
                model.ProfileId = await profileDAL.Add(model);
            else
                await profileDAL.Update(model);
        }

        public async Task<IEnumerable<ProfileModel>> Get(int userId)
        {
            return await profileDAL.GetByUserId(userId);
        }

        public async Task<IEnumerable<ProfileSkillModel>> GetProfileSkills(int profileId)
        {
            return await skillDAL.GetProfileSkills(profileId);
        }

        public async Task AddProfileSkill(ProfileSkillModel model)
        {
            var skill = await this.skillDAL.Get(model.SkillName);
            if (skill == null || skill.SkillId == null)
                model.SkillId = await this.skillDAL.Create(model.SkillName);
            else
                model.SkillId = skill.SkillId ?? 0;
            await skillDAL.AddProfileSkill(model);
        }
    }
}
