import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import AppSettings from './AppSettings';
import { Guid } from 'guid-typescript';
// https://stackoverflow.com/questions/43193049/app-settings-the-angular-way
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OpenquizService {

  constructor(private http: HttpClient) { }
  

  GetQuestionsQuizSession(categoryID : number){

    return this.http.get(environment.openQuizServerURL + "Quiz?categoryID=" + categoryID, { withCredentials: true });
  }

  ReviewAnswerInServer(questionID: Guid, answer: string){
    // let headers = new HttpHeaders();
    // headers = headers.append('Cookie', 'B1SESSION=' + localStorage.getItem('B1SESSION') + '; ROUTEID=.node3');
    // debugger;
    return this.http.get(environment.openQuizServerURL 
      + "Quiz/evaluateAnswer?questionID=" + questionID  +"&userAnswer=" + answer, { withCredentials: true });
  }

}
