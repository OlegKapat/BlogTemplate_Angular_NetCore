import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { CategoryListComponent } from './core/components/features/category/category-list/category-list.component';
import { AddCategoryComponent } from './core/components/features/category/add-category/add-category.component';
import { FormsModule } from '@angular/forms';
import {
  HttpClientModule,
  provideHttpClient,
  withInterceptors,
} from '@angular/common/http';
import { CategoryService } from './core/components/features/category/services/category.service';
import { EditCategoryComponent } from './core/components/features/category/edit-category/edit-category.component';
import { BlogpostListComponent } from './core/components/features/blog-post/blogpost-list/blogpost-list.component';
import { AddBlogpostComponent } from './core/components/features/blog-post/add-blogpost/add-blogpost.component';
import {
  CLIPBOARD_OPTIONS,
  ClipboardButtonComponent,
  MarkdownModule,
} from 'ngx-markdown';
import { EditBlogpostComponent } from './core/components/features/blog-post/edit-blogpost/edit-blogpost.component';
import { ImageSelectorComponent } from './core/components/features/shared/components/image-selector/image-selector.component';
import { HomeComponent } from './core/components/features/public/home/home.component';
import { BlogDetailsComponent } from './core/components/features/public/blog-details/blog-details.component';
import { LoginComponent } from './core/components/features/auth/login/login.component';
import { authInterceptor } from './core/inteceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    CategoryListComponent,
    AddCategoryComponent,
    EditCategoryComponent,
    BlogpostListComponent,
    AddBlogpostComponent,
    EditBlogpostComponent,
    ImageSelectorComponent,
    HomeComponent,
    BlogDetailsComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MarkdownModule.forRoot({
      clipboardOptions: {
        provide: CLIPBOARD_OPTIONS,
        useValue: {
          buttonComponent: ClipboardButtonComponent,
        },
      },
    }),
  ],
  providers: [
    CategoryService,
    provideHttpClient(withInterceptors([authInterceptor])),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
