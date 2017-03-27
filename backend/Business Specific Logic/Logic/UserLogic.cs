using Devcorner.NIdenticon;
using Devcorner.NIdenticon.BrushGenerators;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Reusable.Auth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Reusable
{
    public interface IUserLogic : IBaseLogic<User>
    {
    }

    public class UserLogic : BaseLogic<User>, IUserLogic
    {
        public UserLogic(DbContext context, IRepository<User> repository) : base(context, repository)
        {
        }

        protected override void onCreate(User entity)
        {
            entity.Role = "Usuario";
        }

        protected override void onBeforeSaving(User entity, BaseEntity parent = null)
        {
            if (string.IsNullOrWhiteSpace(entity.Value))
            {
                throw new KnownError("[Nombre para mostrar] es un campo requerido");
            }

            //Is a regular User:
            if (entity.Role != "Supervisor")
            {
                if (string.IsNullOrWhiteSpace(entity.UserName))
                {
                    throw new KnownError("[Gafete] es un campo requerido.");
                }

                if (entity.UserName.Length < 6)
                {
                    throw new KnownError("El gafete debe contener al menos 6 caracteres.");
                }

                entity.Password = entity.UserName;
                entity.ConfirmPassword = entity.UserName;

                entity.Role = "Usuario";
            }
            //Is a supervisor
            else
            {
                if (string.IsNullOrWhiteSpace(entity.UserName))
                {
                    throw new KnownError("[Usuario] es un campo requerido.");
                }
                if (entity.id == 0)
                {
                    if (string.IsNullOrWhiteSpace(entity.Password) || string.IsNullOrWhiteSpace(entity.ConfirmPassword))
                    {
                        throw new KnownError("[Contraseña] y [Confirme Contraseña] son campos requeridos.");
                    }

                    if (entity.Password != entity.ConfirmPassword)
                    {
                        throw new KnownError("No coinciden [Contraseña] con su confirmación");
                    }

                    if (entity.Password.Length < 6)
                    {
                        throw new KnownError("La contraseña debe contener al menos 6 caracteres.");
                    }

                    entity.AuthorizatorPassword = entity.Password;
                }
            }

            //Updating..
            if (entity.id > 0)
            {
                User originalUser = repository.GetByID(entity.id);
                if (originalUser == null)
                {
                    throw new KnownError("Error. No se ha encontrado el usuario a editar (Tabla: Users).");
                }
                using (var authContext = new AuthContext())
                {
                    UserManager<IdentityUser> _userManager;
                    _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authContext));

                    IdentityUser user = authContext.Users.FirstOrDefault(u => u.UserName == originalUser.UserName);
                    if (user == null)
                    {
                        throw new KnownError("Error. No se ha encontrado usuario a editar (Tabla: ASPNetUsers).");
                    }

                    user.UserName = entity.UserName;
                    //password was updated:
                    if (!string.IsNullOrWhiteSpace(entity.Password))
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(entity.Password);
                    }

                    var result = _userManager.Update(user);
                    if (result == null || !result.Succeeded)
                    {
                        throw new KnownError("Ha ocurrido un error al intentar editar el usuario.");
                    }
                }
            }
            //Creating..
            else
            {
                using (var authContext = new AuthContext())
                {
                    UserManager<IdentityUser> _userManager;
                    _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authContext));

                    IdentityUser user = new IdentityUser
                    {
                        UserName = entity.UserName
                    };

                    var result = _userManager.Create(user, entity.Password);
                    if (result == null || !result.Succeeded)
                    {
                        string sError = result.Errors.First();
                        if (sError.EndsWith("is already taken."))
                        {
                            throw new KnownError("Usuario " + entity.UserName + " ya esta tomado.");
                        }
                        else
                        {
                            throw new KnownError(sError);
                        }
                    }
                }
            }

            //Bitmap identicon;
            //try
            //{
            //    var bg = new StaticColorBrushGenerator(StaticColorBrushGenerator.ColorFromText(entity.UserName));
            //    identicon = new IdenticonGenerator("MD5")
            //        .WithSize(100, 100)
            //        .WithBackgroundColor(Color.White)
            //        .WithBlocks(4, 4)
            //        .WithBlockGenerators(IdenticonGenerator.ExtendedBlockGeneratorsConfig)
            //        .WithBrushGenerator(bg)
            //        .Create(entity.UserName);
            //}
            //catch (Exception ex)
            //{
            //    throw new KnownError("Ha ocurrido un error al intentar crear el usuario.");
            //}

            //ImageConverter converter = new ImageConverter();

            //try
            //{
            //    entity.Identicon64 = Convert.ToBase64String(ConvertBitMapToByteArray(identicon));
            //    entity.Identicon = (byte[])converter.ConvertTo(identicon, typeof(byte[]));
            //}
            //catch (Exception ex)
            //{
            //    throw new KnownError("Ha ocurrido un error al intentar crear el usuario");
            //}
        }
        private byte[] ConvertBitMapToByteArray(Bitmap bitmap)
        {
            byte[] result = null;

            if (bitmap != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    result = stream.ToArray();
                }
            }

            return result;
        }

        //Specific for UserLogic
        public CommonResponse GetByName(string sName)
        {
            CommonResponse response = new CommonResponse();
            List<User> entities = new List<User>();
            try
            {
                User entity = repository.GetSingle(e => e.UserName == sName);
                if (entity != null)
                {
                    entities.Add(entity);
                    loadNavigationProperties(entities.ToArray());
                }
                return response.Success(entity);

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        protected override void onBeforeRemoving(User entity, BaseEntity parent = null)
        {
            using (var authContext = new AuthContext())
            {
                UserManager<IdentityUser> _userManager;
                _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authContext));

                IdentityUser user = authContext.Users.FirstOrDefault(u => u.UserName == entity.UserName);
                if (user == null)
                {
                    throw new KnownError("Error. No se ha encontrado usuario a eliminar (Tabla: ASPNetUsers).");
                }

                var result = _userManager.Delete(user);
                if (result == null || !result.Succeeded)
                {
                    throw new KnownError("Ha ocurrido un error al intentar eliminar el usuario.");
                }
            }
        }
    }
}
