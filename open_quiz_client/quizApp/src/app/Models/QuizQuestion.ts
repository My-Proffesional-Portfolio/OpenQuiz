import { Guid } from "guid-typescript";

export interface QuizQuestion {
    correctAnswer: string;
    id: Guid;
    question: string;
    answers: string[];
    selectedAnswer: string;
    isCorrect: boolean;
    isAnswered: boolean;
    sessionID: string;
}