import { Component, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import { QuizQuestion } from 'src/app/Models/QuizQuestion';
import { CategoriesService } from 'src/app/Services/categories.service';
import { OpenquizService } from 'src/app/Services/openquiz.service';


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

  constructor(private catService: CategoriesService, private quizService: OpenquizService) { }

  categoriesList : CategoriesCatalog[] = []
  selectedCategtory : CategoriesCatalog = <CategoriesCatalog>{}
  quizQuestions : QuizQuestion[] = []

  ngOnInit(): void {
    this.catService.getQuizCategories().subscribe((data: any)=> {
      debugger;
      console.log(data)
      this.categoriesList = data.trivia_categories;
      this.selectedCategtory = this.categoriesList[0];
    })
  }


  getSessionQuestions (){
    // alert(this.selectedCategtory.id)
    this.quizService.GetQuestionsQuizSession(this.selectedCategtory.id)
    .subscribe({
      next: (data: any) => {
        this.quizQuestions = data;
        localStorage.setItem("B1SESSION", JSON.stringify(this.quizQuestions[0].sessionID));
      },
      error: (err) => {
        alert(err.statusText);
        },
      });
      
    }

    getSelectedQuestion(questionID: Guid, answerIndex: number){
      debugger;
      var question = this.quizQuestions.find(({id}) => id === questionID);
      var options = question == undefined ? []: question.answers;
      var selectedOption = options[answerIndex];

      for (let index = 0; index < this.quizQuestions.length; index++) {
        
        if (this.quizQuestions[index].id === questionID)
        {
          this.quizQuestions[index].selectedAnswer = selectedOption;
          break;
        }
        
      }
      // if (question != undefined){
      //   question.selectedAnswer = selectedOption;
      // }
    }

    sendAnswerToServer(questionID: Guid){
      debugger;
      var question = this.quizQuestions.find(({id}) => id === questionID);
      if (question != undefined){
       
        this.quizService.ReviewAnswerInServer(questionID, question.selectedAnswer).
        subscribe({
          next: (data: any) => {
            var isCorrect = data;
            for (let index = 0; index < this.quizQuestions.length; index++) {
        
              if (this.quizQuestions[index].id === questionID)
              {
                this.quizQuestions[index].isAnswered = true;
                this.quizQuestions[index].isCorrect = data.isCorrectAnswer;
                break;
              }
              
            }
          },
          error: (err) => {
            alert(err.statusText);
            },
          });


      }
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
