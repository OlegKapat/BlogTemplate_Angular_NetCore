import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.model';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../../category/models/category.model';
import { Router } from '@angular/router';
import { CategoryService } from '../../category/services/category.service';
import { BlogPostService } from '../services/blog-post.service';
import { ImageService } from '../../shared/services/image.service';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.scss',
})
export class AddBlogpostComponent implements OnInit, OnDestroy {
  model: AddBlogPost;
  isImageSelectorVisible: boolean = false;
  categories$?: Observable<Category[]>;

  imageSelectorSubscription?: Subscription;
  constructor(
    private blogPostService: BlogPostService,
    private router: Router,
    private categoryService: CategoryService,
    private imageService: ImageService
  ) {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(),
      categories: []
    }
  }
  
 ngOnInit(): void {
  this.categories$ = this.categoryService.getAllCategories();
  this.imageSelectorSubscription = this.imageService.onSelectImage()
  .subscribe({
   next: (selectedImage) => {
     this.model.featuredImageUrl = selectedImage.url;
     this.closeImageSelector();
   }
  })
 }
  onFormSubmit(): void {
    this.blogPostService.createBlogPost(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/admin/blogposts');
      },
    });
  }
  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector() : void {
    this.isImageSelectorVisible = false;
  }
  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
  }
}
