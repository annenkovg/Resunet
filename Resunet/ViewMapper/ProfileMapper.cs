﻿using ResunetBl.ViewModels;
using ResunetDal.Models;

namespace ResunetBl.ViewMapper
{
    public static class ProfileMapper
    {
        public static ProfileModel MapProfileViewModelToProfileModel(ProfileViewModel model)
        {
            // этом маппере должны быть только те поля, которые user может ввести в UI
            return new ProfileModel()
            {
                ProfileId = model.ProfileId,
                ProfileName = model.ProfileName,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }

        // превращение бэкенд во фронтенд
        public static ProfileViewModel MapProfileModelToProfileViewModel(ProfileModel model)
        {
            return new ProfileViewModel()
            {
                ProfileId = model.ProfileId,
                ProfileName = model.ProfileName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImage = model.ProfileImage
            };
        }
    }
}
