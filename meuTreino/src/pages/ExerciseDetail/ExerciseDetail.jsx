
//  FETCH COM TOKEN - API

import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import fetchWithToken from '../../api/tokenUser';
import abdominal from '../../assets/abdominal.png';
import quadríceps from '../../assets/quadríceps.png';
import styles from './ExerciseDetail.module.css';

//Mapeamento das imagens com base no nome do exercício
//nome do exercício: imagem correspondente
const exerciseImages = {
    costas: abdominal,
    peito: quadríceps,
  };

const ExerciseDetail = () => {
    const { id } = useParams(); // Captura o ID da URL
    const [exercise, setExercise] = useState(null); // Estado para armazenar os dados do exercício

    // Função para buscar os dados do exercício pela API
    const fetchExerciseDetails = async (exerciseId) => {
        try {
            const data = await fetchWithToken(`http://localhost:5000/api/exercicios/${exerciseId}`);
            setExercise(data);
        } catch (error) {
            console.error('Erro ao buscar detalhes do exercício:', error.message);
        }
    };


    // useEffect para buscar os dados ao montar o componente
    useEffect(() => {
        if (id) {
            fetchExerciseDetails(id);
        }
    }, [id]);

    console.log("exercicio",exercise);
    console.log("id",id);

    // Renderiza um loading enquanto os dados não são carregados
    if (!exercise) {
        return <p>Carregando...</p>;
    }

  //Obtém a imagem correspondente ao exercício. Se não houver, usa uma imagem padrão.
  const exerciseImage = exerciseImages[exercise.exercicio.toLowerCase()] || '../../assets/abdominal.png';


    return (
        <main className={styles.containerExercise}>
            <h1>{exercise.exercicio}</h1>
            <img src={exerciseImage} alt={`Imagem do exercício ${exercise.exercicio}`} className={styles.ImgExercise} />
            <div className={styles.contentExercise}>
                <p className={styles.contentItem}>Série: <span>{exercise.serie}</span></p>
                <p className={styles.contentItem}>Repetições: <span>{exercise.repeticoes}</span></p>
                {exercise.tempo && <p className={styles.contentItem}>Tempo: <span>{exercise.tempo} segundos</span></p>}
            </div>
        </main>
    );
};

export default ExerciseDetail;


// FETCH - JSON SERVER
/* 

import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import styles from './ExerciseDetail.module.css';
import abdominal from '../../assets/abdominal.png';
import quadríceps from '../../assets/quadríceps.png';

// Mapeamento das imagens com base no nome do exercício
// nome do exercício: imagem correspondente
const exerciseImages = {
    costas: abdominal,
    peito: quadríceps,
  };

const ExerciseDetail = () => {
  const { id } = useParams(); // Pega o ID da URL
  const [exercise, setExercise] = useState(null); // Estado para guardar os dados do exercício

  // Função para buscar os detalhes do exercício
  const fetchExerciseDetails = async () => {
    try {
      const response = await fetch(`http://localhost:4000/exercicios/${id}`);
      if (!response.ok) {
        throw new Error('Erro na resposta da API');
      }
      const data = await response.json();
      setExercise(data); // Armazena os dados do exercício no estado
    } catch (error) {
      console.error('Erro ao buscar detalhes do exercício:', error.message);
    }
  };

  useEffect(() => {
    if (id) {
      fetchExerciseDetails(); // Faz a busca quando o id estiver disponível
    }
  }, [id]);

  if (!exercise) {
    return <p>Carregando...</p>; // Exibe carregando enquanto os dados não chegam
  }

    // Obtém a imagem correspondente ao exercício. Se não houver, usa uma imagem padrão.
  const exerciseImage = exerciseImages[exercise.exercicio.toLowerCase()] || '../../assets/abdominal.png';


  return (
    <main className={styles.containerExercise}>
      <h1>{exercise.exercicio}</h1>
      <img
          src={exerciseImage}
        alt={`Imagem do exercício ${exercise.exercicio}`}
        className={styles.ImgExercise}
      />
      <div className={styles.contentExercise}>
        <p className={styles.contentItem}>
          Série: <span>{exercise.serie}</span>
        </p>
        <p className={styles.contentItem}>
          Repetições: <span>{exercise.repeticoes}</span>
        </p>
      </div>
    </main>
  );
};

export default ExerciseDetail;
 */