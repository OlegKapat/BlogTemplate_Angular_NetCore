import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './core/components/features/category/category-list/category-list.component';
import { AddCategoryComponent } from './core/components/features/category/add-category/add-category.component';
import { EditCategoryComponent } from './core/components/features/category/edit-category/edit-category.component';
import { BlogpostListComponent } from './core/components/features/blog-post/blogpost-list/blogpost-list.component';
import { AddBlogpostComponent } from './core/components/features/blog-post/add-blogpost/add-blogpost.component';
import { EditBlogpostComponent } from './core/components/features/blog-post/edit-blogpost/edit-blogpost.component';
import { HomeComponent } from './core/components/features/public/home/home.component';
import { BlogDetailsComponent } from './core/components/features/public/blog-details/blog-details.component';
import { LoginComponent } from './core/components/features/auth/login/login.component';
import { authGuard } from './core/components/features/auth/quards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'blog/:url',
    component: BlogDetailsComponent
  },
  {
    path: 'admin/categories',
    component: CategoryListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/categories/add',
    component: AddCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/categories/:id',
    component: EditCategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/blogposts',
    component: BlogpostListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/blogposts/add',
    component: AddBlogpostComponent,
  },
  {
    path: 'admin/blogposts/:id',
    component: EditBlogpostComponent,
    canActivate: [authGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
