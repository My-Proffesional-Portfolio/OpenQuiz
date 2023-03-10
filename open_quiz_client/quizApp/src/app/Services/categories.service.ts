import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private http: HttpClient) { }

  getQuizCategories(){
    return this.http.get("https://opentdb.com/api_category.php")
  }

}
