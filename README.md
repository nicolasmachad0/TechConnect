# TechConnect

# 🚀 TechConnect

O **TechConnect** é uma plataforma web desenvolvida em **ASP.NET Core MVC** com foco no gerenciamento e divulgação de eventos de tecnologia.

O sistema permite cadastrar eventos, categorias, palestrantes e contatos, oferecendo uma experiência moderna, responsiva e organizada para usuários e administradores.

---

# 📸 Preview

O projeto possui:

✅ Página inicial moderna  
✅ Sistema de autenticação  
✅ Controle de acesso para administrador  
✅ Cadastro de eventos  
✅ Relacionamento N:N entre eventos, categorias e palestrantes  
✅ Página de detalhes completa  
✅ Layout responsivo  
✅ Tema dark moderno  
✅ Gerenciamento de contatos  

---

# 🛠️ Tecnologias Utilizadas

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Identity Authentication
- Razor Pages
- HTML5
- CSS3
- JavaScript

---

# 📂 Estrutura do Projeto

```bash
TechConnect/
│
├── Controllers/
├── Models/
├── Views/
├── Data/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── imagens/
│
├── Areas/
│   └── Identity/
│
└── Program.cs
```
---

# 🔐 Sistema de Autenticação

O sistema utiliza o ASP.NET Identity para autenticação de usuários.

Existem dois tipos de acesso:


👤 Usuário comum


Pode:

Visualizar eventos
Visualizar palestrantes
Visualizar detalhes
Enviar contato

Não pode:

Criar eventos
Editar eventos
Excluir informações


👑 Administrador


O administrador possui acesso total ao sistema.

Pode:

Criar eventos
Editar eventos
Excluir eventos
Gerenciar categorias
Gerenciar palestrantes
Gerenciar contatos

---

# 🎯 Funcionalidades

📌 Eventos
Cadastro completo de eventos
Upload de banner/imagem
Data e horário
Local do evento
Descrição completa
Relacionamento com categorias
Relacionamento com palestrantes

🧠 Categorias
Cadastro de categorias
Busca dinâmica
Listagem moderna
Associação aos eventos

🎤 Palestrantes
Cadastro de palestrantes
Foto do palestrante
Empresa
Cargo
Especialidade
Biografia
Associação aos eventos

📨 Contato
Formulário de contato
Gerenciamento de mensagens
Área administrativa

---

# 🔗 Relacionamentos N:N

O projeto utiliza relacionamentos muitos-para-muitos:

EventoCategoria

Relaciona:

-Evento
-Categoria
-EventoPalestrante

Relaciona:

-Evento
-Palestrante
-Tema da palestra

---

# 📄 Página de Detalhes do Evento

A página de detalhes exibe:

✅Nome do evento
✅Banner
✅Descrição completa
✅ Data
✅Horário
✅ Local
✅ Categorias
✅ Lista de palestrantes
✅ Mini currículo
✅ Tema da palestra
✅ Link para detalhes do palestrante

---

# 🎨 Interface

O sistema foi desenvolvido com foco em:

- Design moderno
- Tema dark
- Responsividade
- Melhor experiência visual
- Navegação intuitiva

---

# 📱 Responsividade

O projeto é totalmente responsivo para:

- Desktop
- Tablets
- Smartphones

---

# 👨‍💻 Autor

Desenvolvido por:

Nicolas Machado Fogaça
Larissa Vitoria Mota Rocha
Kauan Campos

Projeto acadêmico desenvolvido para prática de:

- ASP.NET Core MVC
- Entity Framework
- Relacionamentos N:N
- Identity
- CRUD completo
- Responsividade
- Front-end moderno

---
---

