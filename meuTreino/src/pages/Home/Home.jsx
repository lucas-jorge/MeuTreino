// FETCH COM TOKEN - API

import { PlusCircle, XCircle } from "@phosphor-icons/react";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import styles from "./Home.module.css";
import fetchWithToken from '../../api/tokenUser'

const Home = () => {
  const [userId, setUserId] = useState(null);
  const [userExercises, setUserExercises] = useState([]);
  const [availableExercises, setAvailableExercises] = useState([]);

  // Recupera o ID do usuário do localStorage
  const getUserIdFromLocalStorage = () => {
    const id = localStorage.getItem('userId');
    if (id) {
      setUserId(id);
    } else {
      console.error('ID do usuário não encontrado no localStorage');
    }
  };

  // Função para buscar exercícios associados ao usuário
  const fetchUserExercises = async () => {
    if (userId) {
      try {
        const data = await fetchWithToken(`http://localhost:5000/api/usuario/${userId}/exercicios`);
        setUserExercises(data);
      } catch (error) {
        console.error('Erro ao buscar exercícios do usuário:', error.message);
      }
    }
  };

  // Função para buscar todos os exercícios disponíveis
  const fetchAvailableExercises = async () => {
    try {
      const allExercises = await fetchWithToken('http://localhost:5000/api/exercicios');
      
      // Filtra exercícios que ainda não estão associados ao usuário
      const filteredExercises = allExercises.filter(exercise =>
        !userExercises.some(userExercise => userExercise.id === exercise.id)
      );

      setAvailableExercises(filteredExercises);
    } catch (error) {
      console.error('Erro ao buscar exercícios disponíveis:', error.message);
    }
  };

  // Função para excluir um exercício da lista do usuário
  const handleDelete = async (id) => {
    try {
      await fetchWithToken(`http://localhost:5000/api/exercicios/${id}`, { method: 'DELETE' });
      fetchUserExercises(); // Atualiza a lista após excluir
    } catch (error) {
      console.error('Erro ao excluir exercício:', error.message);
    }
  };

  // Função para adicionar um novo exercício à lista do usuário
  const handleAdd = async (exercise) => {
    try {
      await fetchWithToken(`http://localhost:5000/api/Usuario/${userId}/exercicios`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(exercise)
      });
      fetchUserExercises(); // Atualiza a lista após adicionar
    } catch (error) {
      console.error('Erro ao adicionar exercício:', error.message);
    }
  };

  useEffect(() => {
    getUserIdFromLocalStorage(); // Recupera o ID do usuário ao montar o componente
  }, []);

  useEffect(() => {
    if (userId) {
      fetchUserExercises();   // Busca os exercícios associados ao usuário
      fetchAvailableExercises(); // Busca todos os exercícios disponíveis
    }
  }, [userId]);

  return (
    <main className={styles.homeMain}>
      <h1>Olá, Patrícia!</h1>
      <p className={styles.subtitle}>Escolha seu treino do dia:</p>

 {/* Seção para listar os exercícios do usuário */}
      <section className={styles.userList}>
        {userExercises.map((exercicio) => (
          <div key={exercicio.id} className={styles.userList_item}>
              {/* Link para a página de detalhes do exercício */}
            <Link to={`/home/${exercicio.id}`}>
              <p>{exercicio.exercicio}</p>
            </Link>
             {/* Ícone para excluir o exercício */}
            <XCircle size={32} onClick={() => handleDelete(exercicio.id)} />
          </div>
        ))}
      </section>

   {/* Seção para adicionar novos exercícios à lista do usuário */}
      <section className={styles.userListAdd}>
        <h2>Incluir treino:</h2>
        {availableExercises.map((exercicio) => (
          <div
          key={exercicio.id}
          className={styles.userListAdd_item}
          // Adiciona o exercício à lista do usuário ao clicar
            onClick={() => handleAdd({ id: exercicio.id, exercicio: exercicio.exercicio, serie: 3, repeticoes: 12, tempo: 30 })}
          >
            <p>{exercicio.exercicio}</p>
            <PlusCircle size={32} />
          </div>
        ))}
      </section>
    </main>
  );
};

export default Home;

// FETCH - JSON SERVER

// import { PlusCircle, XCircle } from "@phosphor-icons/react";
// import { useEffect, useState } from "react";
// import { Link } from "react-router-dom";
// import styles from "./Home.module.css";

// const Home = () => {
//   const [userId, setUserId] = useState(null);
//   const [userExercises, setUserExercises] = useState([]);
//   const [availableExercises, setAvailableExercises] = useState([]);

//   // Recupera o ID do usuário do localStorage
//   const getUserIdFromLocalStorage = () => {
//     const id = localStorage.getItem('userId');
//     if (id) {
//       setUserId(id);
//     } else {
//       console.error('ID do usuário não encontrado no localStorage');
//     }
//   };

//   // Função para buscar exercícios associados ao usuário
//   const fetchUserExercises = async () => {
//     if (userId) {
//       try {
//         const response = await fetch(`http://localhost:4000/usuarios/${userId}`);
//         if (!response.ok) {
//           throw new Error('Erro na resposta da API');
//         }
//         const user = await response.json();
//         setUserExercises(user.exercicios || []);
//       } catch (error) {
//         console.error('Erro ao buscar exercícios do usuário:', error.message);
//       }
//     }
//   };

//   // Função para buscar todos os exercícios disponíveis
//   const fetchAvailableExercises = async () => {
//     try {
//       const response = await fetch('http://localhost:4000/exercicios');
//       if (!response.ok) {
//         throw new Error('Erro na resposta da API');
//       }
//       const allExercises = await response.json();

//       // Filtra exercícios que ainda não estão associados ao usuário
//       const filteredExercises = allExercises.filter(exercise =>
//         !userExercises.some(userExercise => userExercise.id === exercise.id)
//       );

//       setAvailableExercises(filteredExercises);
//     } catch (error) {
//       console.error('Erro ao buscar exercícios disponíveis:', error.message);
//     }
//   };

//   // Função para excluir um exercício da lista do usuário
//   const handleDelete = async (id) => {
//     try {
//       const response = await fetch(`http://localhost:4000/usuarios/${userId}`);
//       if (!response.ok) {
//         throw new Error('Erro na resposta da API');
//       }
//       const user = await response.json();
//       const updatedExercises = user.exercicios.filter(exercise => exercise.id !== id);
      
//       await fetch(`http://localhost:4000/usuarios/${userId}`, {
//         method: 'PUT',
//         headers: {
//           'Content-Type': 'application/json'
//         },
//         body: JSON.stringify({ ...user, exercicios: updatedExercises })
//       });

//       fetchUserExercises(); // Atualiza a lista após excluir
//     } catch (error) {
//       console.error('Erro ao excluir exercício:', error.message);
//     }
//   };

//   // Função para adicionar um novo exercício à lista do usuário
//   const handleAdd = async (exercise) => {
//     try {
//       const response = await fetch(`http://localhost:4000/usuarios/${userId}`);
//       if (!response.ok) {
//         throw new Error('Erro na resposta da API');
//       }
//       const user = await response.json();
//       const updatedExercises = [...user.exercicios, exercise];
      
//       await fetch(`http://localhost:4000/usuarios/${userId}`, {
//         method: 'PUT',
//         headers: {
//           'Content-Type': 'application/json'
//         },
//         body: JSON.stringify({ ...user, exercicios: updatedExercises })
//       });

//       fetchUserExercises(); // Atualiza a lista após adicionar
//     } catch (error) {
//       console.error('Erro ao adicionar exercício:', error.message);
//     }
//   };

//   useEffect(() => {
//     getUserIdFromLocalStorage(); // Recupera o ID do usuário ao montar o componente
//   }, []);

//   useEffect(() => {
//     if (userId) {
//       fetchUserExercises(); // Busca os exercícios associados ao usuário
//     }
//   }, [userId]);

//   useEffect(() => {
//     fetchAvailableExercises(); // Busca todos os exercícios disponíveis
//   }, [userExercises]);

//   return (
//     <main className={styles.homeMain}>
//       <h1>Olá, Patrícia!</h1>
//       <p className={styles.subtitle}>Escolha seu treino do dia:</p>

//       {/* Seção para listar os exercícios do usuário */}
//       <section className={styles.userList}>
//         {userExercises.map((exercicio) => (
//           <div key={exercicio.id} className={styles.userList_item}>
//             <Link to={`/home/${exercicio.id}`}>
//               <p>{exercicio.exercicio}</p>
//             </Link>
//             <XCircle size={24} onClick={() => handleDelete(exercicio.id)} />
//           </div>
//         ))}
//       </section>

//       {/* Seção para adicionar novos exercícios à lista do usuário */}
//       <section className={styles.userListAdd}>
//         <h2>Incluir treino:</h2>
//         {availableExercises.map((exercicio) => (
//           <div
//             key={exercicio.id}
//             className={styles.userListAdd_item}
//             onClick={() => handleAdd({ ...exercicio, serie: 3, repeticoes: 12, tempo: 30 })}
//           >
//             <p>{exercicio.exercicio}</p>
//             <PlusCircle size={24} />
//           </div>
//         ))}
//       </section>
//     </main>
//   );
// };

// export default Home;



