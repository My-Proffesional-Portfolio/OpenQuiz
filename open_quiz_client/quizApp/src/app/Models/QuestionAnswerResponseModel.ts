// https://www.npmjs.com/package/guid-typescript
import { Guid } from "guid-typescript";

export interface QuestionAnswerResponseModel {
    isCorrectAndwer: boolean;
    yourAnswer: string;
    acomulatedPoints: number;
    isGameFinished: boolean;
    correctAnswer: string;
    questionID: Guid;
}