import { Component, Input } from "@angular/core";
import { PostDto } from "src/app/core/dto/post.dto";

@Component({ selector: 'app-feed', template: '' })
export class FeedStub {
  @Input() inputPosts: PostDto[] = [];
}
