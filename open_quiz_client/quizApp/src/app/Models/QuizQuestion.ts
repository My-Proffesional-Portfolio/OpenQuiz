import { Guid } from "guid-typescript";

export interface QuizQuestion {
    id: Guid;
    question: string;
    answers: string[];
    selectedAnswer: string;
    isCorrect: boolean;
    isAnswered: boolean;
    sessionID: string;
}