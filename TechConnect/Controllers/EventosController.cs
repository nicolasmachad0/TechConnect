using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechConnect.Data;
using TechConnect.Models;
using Microsoft.AspNetCore.Authorization;

namespace TechConnect.Controllers
{
    // Controller responsável pelo gerenciamento de eventos (CRUD + relacionamentos)
    public class EventosController : Controller
    {
        // Contexto do banco de dados da aplicação
        private readonly ApplicationDbContext _context;

        // Injeção de dependência do DbContext
        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista todos os eventos com categorias e palestrantes relacionados
        public async Task<IActionResult> Index()
        {
            var eventos = await _context.Evento
                .Include(e => e.EventoCategorias)
                    .ThenInclude(ec => ec.Categoria)
                .Include(e => e.EventoPalestrantes)
                    .ThenInclude(ep => ep.Palestrante)
                .ToListAsync();

            return View(eventos);
        }

        // Exibe os detalhes de um evento específico com suas relações
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .Include(e => e.EventoCategorias)
                    .ThenInclude(ec => ec.Categoria)
                .Include(e => e.EventoPalestrantes)
                    .ThenInclude(ep => ep.Palestrante)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // Carrega formulário de criação de evento com listas de categorias e palestrantes
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Palestrantes = _context.Palestrante.ToList();

            return View();
        }

        // Salva um novo evento e suas relações (categorias e palestrantes)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Evento evento,
            List<int> CategoriasSelecionadas,
            List<int> PalestrantesSelecionados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();

                // Associação de categorias ao evento
                foreach (var categoriaId in CategoriasSelecionadas)
                {
                    EventoCategoria ec = new EventoCategoria
                    {
                        EventoId = evento.Id,
                        CategoriaId = categoriaId
                    };

                    _context.EventoCategoria.Add(ec);
                }

                // Associação de palestrantes ao evento
                foreach (var palestranteId in PalestrantesSelecionados)
                {
                    EventoPalestrante ep = new EventoPalestrante
                    {
                        EventoId = evento.Id,
                        PalestranteId = palestranteId
                    };

                    _context.EventoPalestrante.Add(ep);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _context.Categoria.ToList();
            ViewBag.Palestrantes = _context.Palestrante.ToList();

            return View(evento);
        }

        // Carrega tela de edição com dados do evento e relacionamentos
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .Include(e => e.EventoCategorias)
                .Include(e => e.EventoPalestrantes)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = await _context.Categoria.ToListAsync();
            ViewBag.Palestrantes = await _context.Palestrante.ToListAsync();

            ViewBag.CategoriasSelecionadas = evento.EventoCategorias
                .Select(ec => ec.CategoriaId)
                .ToList();

            ViewBag.PalestrantesSelecionados = evento.EventoPalestrantes
                .Select(ep => ep.PalestranteId)
                .ToList();

            return View(evento);
        }

        // Atualiza evento e reconfigura relacionamentos (categorias e palestrantes)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Evento evento,
            int[] CategoriasSelecionadas,
            int[] PalestrantesSelecionados)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            var eventoBanco = await _context.Evento
                .Include(e => e.EventoCategorias)
                .Include(e => e.EventoPalestrantes)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventoBanco == null)
            {
                return NotFound();
            }

            // Atualização dos campos principais do evento
            eventoBanco.Nome = evento.Nome;
            eventoBanco.Descricao = evento.Descricao;
            eventoBanco.Data = evento.Data;
            eventoBanco.Horario = evento.Horario;
            eventoBanco.Local = evento.Local;
            eventoBanco.Imagem = evento.Imagem;

            // Remove relações antigas de categorias
            _context.RemoveRange(eventoBanco.EventoCategorias);

            // Adiciona novas categorias selecionadas
            foreach (var categoriaId in CategoriasSelecionadas)
            {
                eventoBanco.EventoCategorias.Add(new EventoCategoria
                {
                    CategoriaId = categoriaId
                });
            }

            // Remove relações antigas de palestrantes
            _context.RemoveRange(eventoBanco.EventoPalestrantes);

            // Adiciona novos palestrantes selecionados
            foreach (var palestranteId in PalestrantesSelecionados)
            {
                eventoBanco.EventoPalestrantes.Add(new EventoPalestrante
                {
                    PalestranteId = palestranteId
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Carrega tela de confirmação de exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // Remove evento do banco de dados
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Evento.FindAsync(id);

            if (evento != null)
            {
                _context.Evento.Remove(evento);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Verifica se o evento existe no banco
        private bool EventoExists(int id)
        {
            return _context.Evento.Any(e => e.Id == id);
        }
    }
}