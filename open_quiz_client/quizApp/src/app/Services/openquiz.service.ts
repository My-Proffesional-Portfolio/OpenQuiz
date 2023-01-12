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

    // https://stackoverflow.com/questions/59738220/getting-different-php-session-id-evey-request-i-make-angular-front-end
    // https://answers.sap.com/questions/13525307/service-layer-send-http-request-in-angular-with-cu.html
    // send session id in header angular
    // https://stackoverflow.com/questions/60045387/sessionid-is-not-getting-stored-in-cookie-in-an-application-with-angular-6-as-fr
    return this.http.get(environment.openQuizServerURL 
      + "Quiz/evaluateAnswer?questionID=" + questionID  +"&userAnswer=" + answer, { withCredentials: true });
  }

}
