import { PostDto } from "src/app/core/dto/post.dto";

export const mockPostDto: PostDto = {
  postID: '1',
  clientGuid: 'abc123',
  ownerUsername: 'testUser',
  postDescription: 'Test post',
  totalComments: 0,
  isDeletable: true,
  totalLikes: 5,
  likedByUsers: [],
  allComments: [],
  imagePaths: [],
  publishDate: new Date(),
  savedByUsers: [],
  location: 'Test City'
};