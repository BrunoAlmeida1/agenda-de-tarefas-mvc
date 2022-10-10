using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    public class TarefaController : Controller
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound();

            return View(tarefa);
        }

        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();
            return View(tarefas);
        }

        public IActionResult ObterPorTitulo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));

            return View(tarefa);
        }
        public IActionResult ObterPorData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return View(tarefa);
        }
        public IActionResult ObterPorStatus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return View(tarefa);
        }
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            if (ModelState.IsValid)
            {
                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();
                return RedirectToAction(nameof(ObterTodos));
            }
            return View(tarefa);
        }
        public IActionResult Atualizar(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return RedirectToAction(nameof(ObterTodos));

            return View(tarefa);
        }

        [HttpPost]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(ObterTodos));
        }
        public IActionResult Deletar(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if(tarefa == null)
                return RedirectToAction(nameof(ObterTodos));
            
            return View(tarefa);
        }

        [HttpPost]
        public IActionResult Deletar(Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(tarefa.Id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(ObterTodos));
        }

        public IActionResult Pesquisar()
        {
            return View();
        }
    }
}
