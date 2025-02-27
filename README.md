# Meu Treino

**Aplicativo para prática de exercícios físicos em casa**

## Descrição

O Meu Treino é um aplicativo que visa democratizar o acesso à prática de exercícios físicos, oferecendo uma plataforma completa e personalizada para treinar em casa. Com ele, você pode realizar treinos adaptados às suas necessidades e objetivos, acompanhando seu progresso e definindo metas.

## Funcionalidades

* **Treinos Personalizados:** Oferece treinos adaptados ao seu nível de condicionamento físico e objetivos.
* **Exercícios para Diferentes Grupos Musculares:** Trabalhe todos os músculos do seu corpo com exercícios específicos.
* **Vídeos Demonstrativos:** Aprenda a executar os exercícios corretamente com vídeos instrutivos.
* **Acompanhamento do Progresso:** Monitore seu desenvolvimento com gráficos e relatórios.
* **Definição de Metas:** Estabeleça metas e acompanhe sua evolução.
* **Interface Intuitiva:** Navegue facilmente pelo aplicativo com um design simples e amigável.

## Tecnologias Utilizadas

* **Backend:** Flask (Python)
* **Frontend:** React
* **Banco de Dados:** MySQL

## Código

### 1. DDL (Data Definition Language) para o Banco de Dados

```sql
-- Tabela de Usuários
CREATE TABLE Usuarios (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Senha VARCHAR(255) NOT NULL,
    NivelCondicionamento ENUM('Iniciante', 'Intermediario', 'Avancado')
);

-- Tabela de Exercícios
CREATE TABLE Exercicios (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(255) NOT NULL,
    Descricao TEXT,
    GrupoMuscular VARCHAR(255),
    NivelDificuldade ENUM('Iniciante', 'Intermediario', 'Avancado'),
    LinkVideo TEXT
);

-- Tabela de Treinos
CREATE TABLE Treinos (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(255) NOT NULL,
    Descricao TEXT,
    Duracao INT,
    ID_Usuario INT,
    FOREIGN KEY (ID_Usuario) REFERENCES Usuarios(ID)
);

-- Tabela de Exercícios do Treino
CREATE TABLE ExerciciosTreino (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    ID_Treino INT,
    ID_Exercicio INT,
    Repeticoes INT,
    Series INT,
    FOREIGN KEY (ID_Treino) REFERENCES Treinos(ID),
    FOREIGN KEY (ID_Exercicio) REFERENCES Exercicios(ID)
);

-- Tabela de Histórico de Treinos
CREATE TABLE HistoricoTreinos (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    ID_Usuario INT,
    ID_Treino INT,
    DataTreino DATETIME,
    FOREIGN KEY (ID_Usuario) REFERENCES Usuarios(ID),
    FOREIGN KEY (ID_Treino) REFERENCES Treinos(ID)
);
2. Servidor Backend (Flask)
Python

from flask import Flask, request, jsonify
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://usuario:senha@localhost/meu_treino'
db = SQLAlchemy(app)

# Definição dos modelos (Usuário, Exercício, Treino, etc.)
# ...

# Endpoint para cadastro de usuário
@app.route('/cadastro', methods=['POST'])
def cadastro():
    dados = request.get_json()
    # Lógica para validar os dados e criar um novo usuário
    # ...
    return jsonify({'mensagem': 'Usuário cadastrado com sucesso!'})

# Endpoint para login de usuário
@app.route('/login', methods=['POST'])
def login():
    dados = request.get_json()
    # Lógica para autenticar o usuário
    # ...
    return jsonify({'token': 'token_de_acesso'})

# Outros endpoints para criação de treinos, busca de exercícios, etc.
# ...

if __name__ == '__main__':
    app.run(debug=True)
3. Servidor Frontend (React)
JavaScript

import React, { useState } from 'react';

function App() {
  const [treinos, setTreinos] = useState();

  // Função para buscar treinos do backend
  const buscarTreinos = async () => {
    const response = await fetch('/treinos');
    const data = await response.json();
    setTreinos(data);
  };

  return (
    <div>
      <h1>Meu Treino</h1>
      <button onClick={buscarTreinos}>Buscar Treinos</button>
      <ul>
        {treinos.map((treino) => (
          <li key={treino.id}>{treino.nome}</li>
        ))}
      </ul>
    </div>
  );
}

export default App;

# Como Usar

Clone o repositório: git clone [inserir link do repositório aqui]
Instale as dependências: npm install
Configure o banco de dados: crie um banco de dados MySQL e importe o arquivo meu_treino.sql.
Inicie o servidor backend: python app.py
Inicie o servidor frontend: npm start

# Equipe
Lucas Albuquerque Jorge
Marco Antonio Casanova
Tiago Tadashi Leche
Yara Corrêa de Sá

# Links

Landing Page: [inserir link da landing page aqui]
Vídeo Demonstrativo: [inserir link do vídeo aqui]
