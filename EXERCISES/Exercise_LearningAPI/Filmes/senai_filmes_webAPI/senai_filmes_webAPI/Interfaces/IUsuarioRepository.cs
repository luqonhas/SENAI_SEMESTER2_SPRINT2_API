﻿using senai_filmes_webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai_filmes_webAPI.Interfaces
{
    /// <summary>
    /// Interface responsável pelo repositório UsuarioRepository
    /// </summary>
    interface IUsuarioRepository
    {
        /// <summary>
        /// Valida o usuário
        /// </summary>
        /// <param name="email"> recebe o email do usuário </param>
        /// <param name="senha"> recebe a senha do usuário </param>
        /// <returns> objeto do tipo "UsuarioDomain" que foi buscado </returns>
        UsuarioDomain BuscarPorEmailSenha(string email, string senha);
    }
}
