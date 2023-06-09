﻿using System.ComponentModel.DataAnnotations;

namespace JobReady;

public class PostDetails
{
    public long Id { get; set; }
    public string Content { get; set; }
    public IFormFile Image { get; set; }
    public long? ImageId { get; set; }
    public string CreatedById { get; set; }
    public long LikesCount { get; set; }
    public bool HasLiked { get; set; }
    public IEnumerable<PostEngagementDetails> Comments { get; set; }
    public UserAccountDetails CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string PostedOn { get; set; }

}
