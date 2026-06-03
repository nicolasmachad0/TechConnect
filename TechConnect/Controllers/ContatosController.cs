using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechConnect.Data;
using TechConnect.Models;

namespace TechConnect.Controllers
{
    // Controller responsável pelo gerenciamento de contatos (mensagens enviadas pelos usuários)
    public class ContatosController : Controller
    {
        // Contexto do banco de dados da aplicação
        private readonly ApplicationDbContext _context;

        // Injeção de dependência do DbContext
        public ContatosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista todos os contatos ordenados pela data de envio (mais recentes primeiro)
        public async Task<IActionResult> Index()
        {
            var contatos = await _context.Contato
                .OrderByDescending(c => c.DataEnvio)
                .ToListAsync();

            return View(contatos);
        }

        // Exibe os detalhes de um contato específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // Carrega o formulário de criação de contato
        public IActionResult Create()
        {
            return View();
        }

        // Salva um novo contato no banco de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Assunto,Mensagem")] Contato contato)
        {
            if (ModelState.IsValid)
            {
                // Define a data de envio automaticamente no servidor
                contato.DataEnvio = DateTime.Now;

                _context.Add(contato);
                await _context.SaveChangesAsync();

                // Redireciona para tela de confirmação após envio
                return RedirectToAction(nameof(ContatoEnviado));
            }

            return View(contato);
        }

        // Tela de confirmação após envio do contato
        public IActionResult ContatoEnviado()
        {
            return View();
        }

        // Carrega o formulário de edição de contato
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato.FindAsync(id);

            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // Atualiza os dados do contato
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Assunto,Mensagem,DataEnvio")] Contato contato)
        {
            if (id != contato.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatoExists(contato.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(contato);
        }

        // Carrega a tela de confirmação de exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // Remove o contato do banco de dados
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contato = await _context.Contato.FindAsync(id);

            if (contato != null)
            {
                _context.Contato.Remove(contato);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Verifica se o contato existe no banco de dados
        private bool ContatoExists(int id)
        {
            return _context.Contato.Any(e => e.Id == id);
        }
    }
}