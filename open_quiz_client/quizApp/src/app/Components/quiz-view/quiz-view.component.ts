import { Component, OnInit } from '@angular/core';
import { CategoriesService } from 'src/app/Services/categories.service';

interface CategoriesCatalog {
  id: number;
  name : string
}


@Component({
  selector: 'app-quiz-view',
  templateUrl: './quiz-view.component.html',
  styleUrls: ['./quiz-view.component.css']
})
export class QuizViewComponent implements OnInit {

  constructor(private catService: CategoriesService) { }

  categoriesList : CategoriesCatalog[] = []
  selectedCategtory : CategoriesCatalog = <CategoriesCatalog>{}

  ngOnInit(): void {
    this.catService.getQuizCategories().subscribe((data: any)=> {
      debugger;
      console.log(data)
      this.categoriesList = data.trivia_categories;
      this.selectedCategtory = this.categoriesList[0];
    })
  }


  showCategoryID (){
    alert(this.selectedCategtory.id)
  }
  // contactMethods = [
  //   { id: 1, label: "Email" },
  //   { id: 2, label: "Phone" }
  // ]
  

  // contact = {
  //   firstName: "CFR",
  //   comment: "No comment",
  //   subscribe: true,
  //   contactMethod: 2 // this id you'll send and get from backend
  // }

  
}
