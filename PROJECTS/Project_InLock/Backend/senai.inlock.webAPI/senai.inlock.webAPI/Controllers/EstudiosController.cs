﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.inlock.webAPI.Domains;
using senai.inlock.webAPI.Interfaces;
using senai.inlock.webAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.inlock.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosController : ControllerBase
    {
        private IEstudioRepository _estudioRepository { get; set; }

        private IJogoRepository _jogoRepository { get; set; }

        public EstudiosController()
        {
            _estudioRepository = new EstudioRepository();
            _jogoRepository = new JogoRepository();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            List<EstudioDomain> listaEstudios = _estudioRepository.Listar();

            return Ok(listaEstudios);
        }

        [HttpGet("extra")]
        public IActionResult GetExtra()
        {
            // lista com o método do Listar do repositório dos Estúdios
            List<EstudioDomain> listaEstudiosJogos = _estudioRepository.Listar();

            // lista objeto vazia
            List<object> listaObject = new List<object>();

            // o foreach vai executar um estúdio de cada vez
            // vai executar os itens do EstudioDomain que estão na listaEstudiosJogos (a lista de ListarTodos no EstudioRepository)
            // por exemplo, o foreach vai pegar um estúdio de cada vez e o primeiro estúdio por exemplo será a Blizzard
            foreach (EstudioDomain item in listaEstudiosJogos)
            {
                // uma lista com o JogoDomain(jogos) que vai conter um método de listar os jogos por id(ListarJogos(int id)) que foi criado para esse extra, dentro dos argumentos desse ListarJogos será necessário colocar o id do Estudio que foi colocado na query do método(WHERE jogos.idEstudio = @id)
                List<JogoDomain> jogos = _jogoRepository.ListarJogos(item.idEstudio);

                // é criado um objeto "obj" que irá juntar os atributos(idEstudio e nomeEstudio), onde será colocado os itens da EstudioDomain da listaEstudiosJogos, e será juntado com a lista do JogoDomain(jogos) que foi criada acima
                // no object "obj", será colocado os itens id do estúdio(1) e o seu nome(Blizzard), e também será adicionada a lista "jogos" que contém os jogos que foram feitos pela Blizzard
                object obj = new { item.idEstudio, item.nomeEstudio, jogos };

                // aqui será adicionada dentro da lista vazia que foi criada no começo esse objeto "obj" que contém as duas listas(com o idEstudio e nomeEstudio de EstudioDomain e a lista "jogos" com o método ListarJogos)
                listaObject.Add(obj);
            }
            
            // aqui retornará a lista com tudo que foi pedido no extra
            return Ok(listaObject);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public IActionResult PutIdUrl(int id, EstudioDomain estudioAtualizado)
        {
            EstudioDomain estudioBuscado = _estudioRepository.BuscarPorId(id);

            if (estudioBuscado == null)
            {
                return NotFound(new { mensagem = "Estúdio não encontrado!" });
            }

            try
            {
                _estudioRepository.Atualizar(id, estudioAtualizado);

                return NoContent();
            }

            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // cria um objeto "funcionarioBuscado" que irá receber o "funcionarioBuscado" no banco de dados
            EstudioDomain estudioBuscado = _estudioRepository.BuscarPorId(id);
            // um "=" é atribuição, um "==" é uma comparação

            // verifica se nenhum funcionário foi encontrado
            if (estudioBuscado == null)
            {
                // caso não seja encontrado, retorna um status code 404 - Not Found com uma mensagem personalizada
                return NotFound("Nenhum estúdio encontrado!");
            }

            // caso seja encontrado, retorna o funcionário buscado com um status code 200 - Ok
            return Ok(estudioBuscado);
        }

        [Authorize]
        [HttpGet("buscar/{buscado}")]
        public IActionResult GetByName(string buscado)
        {
            EstudioDomain estudioBuscado = _estudioRepository.BuscarPorNome(buscado);

            if (estudioBuscado == null)
            {
                return NotFound("Nenhum estúdio encontrado!");
            }
            else
                return Ok(estudioBuscado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Post(EstudioDomain novoEstudio)
        {
            try // tenta executar...
            {
                // se o conteúdo do nome e/ou do sobrenome do novo funcionário estar vazio ou com um espaço em branco...
                if (String.IsNullOrWhiteSpace(novoEstudio.nomeEstudio))
                {
                    // retorna um status code 404 - Not Found com uma mensagem personalizada
                    return NotFound("Campo 'nome' obrigatório!");
                }

                // se estiver tudo preenchido...
                else
                    // faz a chamada para o método Cadastrar
                    _estudioRepository.Cadastrar(novoEstudio);

                // e retorna o status code 201 - Created
                return StatusCode(201);
            }

            // se não conseguiu executar...
            catch (Exception codErro)
            {
                // retorna um status code 400 - BadRequest e o código do erro
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // faz a chamada para o método .Deletar
            _estudioRepository.Deletar(id);

            // retorna o status code 204 - No Content
            return StatusCode(204);
        }


    }
}
