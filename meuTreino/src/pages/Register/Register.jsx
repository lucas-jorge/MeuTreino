
import styles from './Register.module.css'; // Importando o CSS Module

const Register = () => {
  return (
    <div className={styles.container}>
      <h1 className={styles.logo}>MEU TREINO</h1>
      <div className={styles.register}>
        <h2>Crie sua Conta</h2>
        <form id="registerForm">
          <label htmlFor="name">Nome Completo:</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-user ${styles.inputIcon}`}></i>
            <input type="text" id="name" name="name" required />
          </div>

          <label htmlFor="email">Email:</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-envelope ${styles.inputIcon}`}></i>
            <input type="email" id="email" name="email" required />
          </div>

          <label htmlFor="password">Senha:</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-lock ${styles.inputIcon}`}></i>
            <input type="password" id="password" name="password" required />
          </div>

          <label htmlFor="confirmPassword">Confirmar Senha:</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-lock ${styles.inputIcon}`}></i>
            <input type="password" id="confirmPassword" name="confirmPassword" required />
          </div>

          <label htmlFor="gender">Gênero:</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-venus-mars ${styles.inputIcon}`}></i>
            <select id="gender" name="gender" required>
              <option value="Masculino">Masculino</option>
              <option value="Feminino">Feminino</option>
              <option value="Não Informar">Prefiro não informar</option>
            </select>
          </div>

          <label htmlFor="level">Nível de Treino:</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-dumbbell ${styles.inputIcon}`}></i>
            <select id="level" name="level" required>
              <option value="Iniciante">Iniciante</option>
              <option value="Intermediário">Intermediário</option>
              <option value="Avançado">Avançado</option>
            </select>
          </div>

          <label htmlFor="birthdate">Data de Nascimento (opcional):</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-calendar-alt ${styles.inputIcon}`}></i>
            <input type="date" id="birthdate" name="birthdate" />
          </div>

          <label htmlFor="goal">Objetivo do Treino (opcional):</label>
          <div className={styles.inputGroup}>
            <i className={`fas fa-bullseye ${styles.inputIcon}`}></i>
            <input type="text" id="goal" name="goal" />
          </div>

          <label className={styles.termsLabel}>
            <input type="checkbox" id="terms" name="terms" required /> Concordo com os Termos de Uso e a Política de Privacidade
          </label>

          <button type="submit">Cadastrar</button>
        </form>

        <p>Já tem uma conta? <a href="../login/login.html">Faça login</a></p>
      </div>
    </div>
  );
};

export default Register;
