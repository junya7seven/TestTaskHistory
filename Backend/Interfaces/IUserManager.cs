using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IUserManager
    {
        /// <summary>
        /// получить список всех пользователей с паггинацией
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<User>?> GetAll(int currentPage, int pageSize);

        /// <summary>
        /// получить список пользователей по ФИО с паггинацией
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<User>?> GetByFullName(string fullName,
            int currentPage, int pageSize);

        /// <summary>
        /// получить пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User?> GetById(string id);

        /// <summary>
        /// создать пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> Create(User user);

        /// <summary>
        /// обновить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> Update(User user);

        /// <summary>
        /// удалить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Delete(User user);
    }
}
