import { Injectable } from '@angular/core';
import { PostDto } from '../../dto/post.dto';

@Injectable({
  providedIn: 'root'
})
export class IndexedDbService{
  // posts!: Dexie.Table<PostDto, number>;
  // constructor() { 
  //   super("PixelNestDb");
  //   this.version(1).stores({
  //     posts: 'postID, userID, ownerUsername, totalLikes, totalComments, location, publishDate'
  //   })
  //   this.posts = this.table('posts');
  // }
  // async addPost(post: PostDto) {
  //   return await this.posts.put(post);
  // }

  // async getPosts(): Promise<PostDto[]> {
  //   return await this.posts.toArray();
  // }

  // async clearPosts() {
  //   return await this.posts.clear();
  // }
}
