
// import { useState } from 'react';
// import styles from './Login.module.css'; // Importando o CSS Module

// const Login = () => {
//   const [username, setUsername] = useState(''); // Estado para o nome de usuário
//   const [password, setPassword] = useState(''); // Estado para a senha
//   const [error, setError] = useState(null); // Estado para lidar com erros de login
//   const [loading, setLoading] = useState(false); // Estado para controlar o loading

//   const handleLogin = async (e) => {
//     e.preventDefault(); // Prevenir o comportamento padrão do formulário
//     setLoading(true);
//     setError(null);

//     try {
//       const response = await fetch('http://localhost:5000/api/Usuario/Login', {
//         method: 'POST',
//         headers: {
//           'Content-Type': 'application/json',
//         },
//         body: JSON.stringify({ nome: username, senha: password }),
//       });

//       if (!response.ok) {
//         throw new Error('Usuário ou senha incorretos');
//       }

//       const data = await response.json();
//       localStorage.setItem('token', data.token); // Armazena o token no localStorage
//       console.log('Login bem-sucedido, token:', data.token);

//       // Aqui você pode redirecionar o usuário para outra página
//       // Exemplo: window.location.href = '/dashboard';

//     } catch (err) {
//       setError(err.message);
//     } finally {
//       setLoading(false);
//     }
//   };

//   return (
//     <div className={styles.container}>
//       <h1 className={styles.logo}>MEU TREINO</h1>
//       <div className={styles.login}>
//         <h2>Login</h2>
//         <p>Acesse seu cadastro para continuar</p>
//         <form id="login-form" onSubmit={handleLogin}>
//           <div className={styles.inputGroup}>
//             <i className={`fas fa-user ${styles.inputIcon}`}></i>
//             <input
//               type="text"
//               placeholder="Usuário"
//               value={username}
//               onChange={(e) => setUsername(e.target.value)} // Atualiza o estado com o valor do input
//               required
//             />
//           </div>
//           <div className={styles.inputGroup}>
//             <i className={`fas fa-lock ${styles.inputIcon}`}></i>
//             <input
//               type="password"
//               placeholder="Senha"
//               value={password}
//               onChange={(e) => setPassword(e.target.value)} // Atualiza o estado com o valor do input
//               required
//             />
//           </div>
//           {error && <p className={styles.error}>{error}</p>}
//           <p>
//             Você ainda não tem uma conta?{' '}
//             <a href="/cadastro">Cadastre-se</a>
//           </p>
//           <button type="submit" className={styles.btnLogin} disabled={loading}>
//             {loading ? 'Carregando...' : 'Login'}
//           </button>
//         </form>
//       </div>
//     </div>
//   );
// };

// export default Login;

import { useState } from 'react';
// import { useNavigate } from 'react-router-dom'; // Importa o useNavigate
import styles from './Login.module.css'; // Importando o CSS Module

const Login = () => {
  const [username, setUsername] = useState(''); // Estado para o nome de usuário
  const [password, setPassword] = useState(''); // Estado para a senha
  const [error, setError] = useState(null); // Estado para lidar com erros de login
  const [loading, setLoading] = useState(false); // Estado para controlar o loading
  // const navigate = useNavigate(); // Inicializa o hook useNavigate

  const handleLogin = async (e) => {
    e.preventDefault(); // Prevenir o comportamento padrão do formulário
    setLoading(true);
    setError(null);
  
    try {
      const response = await fetch('http://localhost:5000/api/Usuario/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ nome: username, senha: password }),
      });
  
      if (!response.ok) {
        throw new Error('Usuário ou senha incorretos');
      }
  
      const data = await response.json();
      localStorage.setItem('token', data.token); // Armazena o token no localStorage
      localStorage.setItem('userId', data.id); // Armazena o ID do usuário no localStorage
      console.log('Login bem-sucedido, token:', data.token, 'ID:', data.id);
  
      // Redireciona para a home após o login
      window.location.href = '/home';
  
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className={styles.loginBody}>
      <div className={styles.container}>
      <h1 className={styles.logo}>MEU TREINO</h1>
      <div className={styles.login}>
        <h2>Login</h2>
        <p>Acesse seu cadastro para continuar</p>
        <form id="login-form" onSubmit={handleLogin}>
          <div className={styles.inputGroup}>
            <i className={`fas fa-user ${styles.inputIcon}`}></i>
            <input
              type="text"
              placeholder="Usuário"
              value={username}
              onChange={(e) => setUsername(e.target.value)} // Atualiza o estado com o valor do input
              required
            />
          </div>
          <div className={styles.inputGroup}>
            <i className={`fas fa-lock ${styles.inputIcon}`}></i>
            <input
              type="password"
              placeholder="Senha"
              value={password}
              onChange={(e) => setPassword(e.target.value)} // Atualiza o estado com o valor do input
              required
            />
          </div>
          {error && <p className={styles.error}>{error}</p>}
          <p>
            Você ainda não tem uma conta?{' '}
            <a href="/cadastro">Cadastre-se</a>
          </p>
          <button type="submit" className={styles.btnLogin} disabled={loading}>
            {loading ? 'Carregando...' : 'Login'}
          </button>
        </form>
      </div>
    </div>
    </section>
    
  );
};

export default Login;

