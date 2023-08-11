using ASNClub.Data.Models;
using ASNClub.Data.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Configurations
{
    public class ImgUrlConfiguration : IEntityTypeConfiguration<ImgUrl>
    {
        public void Configure(EntityTypeBuilder<ImgUrl> builder)
        {
            builder.HasData(this.GenerateImgUrls());
        }
        private ImgUrl[] GenerateImgUrls()
        {
            ICollection<ImgUrl> ImgUrls = new HashSet<ImgUrl>();

            ImgUrl imgUrl = new ImgUrl()
            {
                Id=1,
                Url= "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL.png"
            };
            ImgUrls.Add(imgUrl);
            imgUrl = new ImgUrl()
            {
                Id = 2,
                Url = "http://www.gunsgripasn.com/images/chireni/chireni_pistolet_WALTHER%20PPK-2.png"
            };
            ImgUrls.Add(imgUrl);
            imgUrl = new ImgUrl()
            {
                Id = 3,
                Url = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL.png"
            };
            ImgUrls.Add(imgUrl);

            imgUrl = new ImgUrl()
            {
                Id = 4,
                Url = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL-1.png"
            };
            ImgUrls.Add(imgUrl);

            imgUrl = new ImgUrl()
            {
                Id = 5,
                Url = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL-2.png"
            };
            ImgUrls.Add(imgUrl);

            imgUrl = new ImgUrl()
            {
                Id = 6,
                Url = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL-4.png"
            };
            ImgUrls.Add(imgUrl);

            imgUrl = new ImgUrl()
            {
                Id = 7,
                Url = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL-7.png"
            };
            ImgUrls.Add(imgUrl);

            imgUrl = new ImgUrl()
            {
                Id = 8,
                Url = "http://www.gunsgripasn.com/images/spare_parts/Dano_palnitel_TT-33.png"
            };
            ImgUrls.Add(imgUrl);

            imgUrl = new ImgUrl()
            {
                Id = 9,
                Url = "http://www.gunsgripasn.com/images/spare_parts/dano_palnitel_Makarov.png"
            };
            ImgUrls.Add(imgUrl);

            return ImgUrls.ToArray();
        }
    }
}
