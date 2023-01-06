import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { QuizViewComponent } from './Components/quiz-view/quiz-view.component';

const routes: Routes = [
  { path: 'quiz', component: QuizViewComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
