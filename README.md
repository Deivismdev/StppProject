# Albums
## GET (list)
### /api/album
Description: Get a list of albums.\
Response Code: 200 (OK)\
Response Body: List of AlbumDto.
## GET (one)
### /api/album/{albumId}
Description: Get a specific album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
Response Code: 200 (OK) if found, 404 (Not Found) if not found.\
Response Body: AlbumDto.
## POST 
### /api/album
Description: Create a new album.\
Request Body: AlbumDto.\
Response Code: 201 (Created) if successful.\
Response Body: AlbumDto.
## PUT 
### /api/album/{albumId}
Description: Edit an existing album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
Request Body: AlbumDto.\
Response Code: 200 (OK) if successful.\
Response Body: AlbumDto.
## DELETE 
### /api/album/{albumId}
Description: Remove an album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
Response Code: 204 (No Content) if successful.
# Album Images
## GET (list)
### /api/album/{albumId}/images
Description: Get a list of images in a specific album.\
Request Parameters:\
albumId (int): The ID of the album.\
Response Code: 200 (OK).\
Response Body: List of ImageDto.
## GET (one)
### /api/album/{albumId}/images/{imageId}
Description: Get a specific image in an album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
Response Code: 200 (OK) if found, 404 (Not Found) if not found.\
Response Body: ImageDto.
## POST 
### /api/album/{albumId}/images
Description: Create a new image in an album.\
Request Parameters:\
albumId (int): The ID of the album.\
Request Body: ImageDto.\
Response Code: 201 (Created) if successful.\
Response Body: ImageDto.
## PUT
### /api/album/{albumId}/images/{imageId}
Description: Edit an existing image in an album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
Request Body: ImageDto.\
Response Code: 200 (OK) if successful.\
Response Body: ImageDto.
## DELETE 
### /api/album/{albumId}/images/{imageId}
Description: Remove an image from an album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
Response Code: 204 (No Content) if successful.
# Image Comments
## GET (list)
### /api/album/{albumId}/images/{imageId}/comments
Description: Get a list of comments on a specific image in an album.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
Response Code: 200 (OK).\
Response Body: List of CommentDto.
## GET (one)
### /api/album/{albumId}/images/{imageId}/comments/{commentId}
Description: Get a specific comment on an image by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
commentId (int): The ID of the comment.\
Response Code: 200 (OK) if found, 404 (Not Found) if not found.\
Response Body: CommentDto.
## POST 
### /api/album/{albumId}/images/{imageId}/comments
Description: Create a new comment on an image in an album.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
Request Body: CommentDto.\
Response Code: 201 (Created) if successful.\
Response Body: CommentDto.
## PUT
### /api/album/{albumId}/images/{imageId}/comments/{commentId}
Description: Edit an existing comment on an image in an album by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
commentId (int): The ID of the comment.\
Request Body: CommentDto.\
Response Code: 200 (OK) if successful.\
Response Body: CommentDto.
## DELETE 
### /api/album/{albumId}/images/{imageId}/comments/{commentId}
Description: Remove a comment on an image by ID.\
Request Parameters:\
albumId (int): The ID of the album.\
imageId (int): The ID of the image.\
commentId (int): The ID of the comment.\
Response Code: 204 (No Content) if successful.
# Request and Response Bodies:

## AlbumDto:
Title (string)\
Description (string)\
## ImageDto:
Title (string)\
Url (string)\
Description (string)\
## CommentDto:
Body (string)