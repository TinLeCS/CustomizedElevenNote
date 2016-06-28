using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class ProfileService
    {
        private readonly string _email;

        public ProfileService(string email)
        {
            _email = email;
        }

        public ProfileViewModel GetProfile()
        {
            ProfileEntity entity;

            using (var ctx = new ElevenNoteDbContext())
            {
                entity =
                    ctx
                        .Profiles
                        .SingleOrDefault(e => e.Email == _email);
            }
            if (entity == null)
                entity = UpdateEmail();

            return
                new ProfileViewModel
                {
                    Email = entity.Email,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Address = entity.Address,
                    PhoneNumber = entity.PhoneNumber
                };   
        }

        public bool UpdateProfile(ProfileViewModel vm)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    ctx
                        .Profiles
                        .SingleOrDefault(e => e.Email == vm.Email);

                entity.FirstName = vm.FirstName;
                entity.LastName = vm.LastName;
                entity.Address = vm.Address;
                entity.PhoneNumber = vm.PhoneNumber;

                return ctx.SaveChanges() == 1;
            }
        }

        public ProfileEntity UpdateEmail()
        {
            ProfileEntity entity;

            using (var ctx = new ElevenNoteDbContext())
            {
                entity =
                    new ProfileEntity
                    {
                        Email = _email
                    };

                ctx.Profiles.Add(entity);
                ctx.SaveChanges();
            }

            return entity;
        }

    }
}
