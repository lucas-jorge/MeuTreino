# Modelo Físico de Banco de Dados

## Diagrama ER

+---------------+ +----------------+ +---------------+
| TB_USUARIO | | TB_Exercicios | | TB_CATEGORIA |
+---------------+ +----------------+ +---------------+
| Id (PK) | | Id (PK) | | Id (PK) |
| Nome | | Exercicio | | Nome |
| Senha | | Serie | +---------------+
| Status | | Repeticoes | |
| Created | | Tempo | |
+---------------+ | UsuarioId (FK) | |
| | Categoria