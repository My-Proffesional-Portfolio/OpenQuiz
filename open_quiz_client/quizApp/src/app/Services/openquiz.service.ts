import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import AppSettings from './AppSettings';

@Injectable({
  providedIn: 'root'
})
export class OpenquizService {

  constructor(private http: HttpClient) { }

  GetQuestionsQuizSession(categoryID : number){

    return this.http.get(AppSettings.openQuizServerURL + "Quiz?categoryID=" + categoryID);
  }

}
