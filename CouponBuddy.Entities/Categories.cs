using CouponBuddy.Entities.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public static class Categories
    {
        [CategoryInfo("Food", "Food & Restaurants")]
        public const int FOOD = 1;
        [CategoryInfo("Beaches", "Beaches & Resorts")]
        public const int BEACH = 2;
        [CategoryInfo("Activities", "Activities")]
        public const int ACTIVITIES = 3;
        [CategoryInfo("Entertainment", "Entertainment")]
        public const int ENTERTAINMENT = 4;

        [CategoryInfo("Specialties", "Gifts & Specialties")]
        public const int SPECIALTIES = 5;
        [CategoryInfo("Boutiques", "Boutiques")]
        public const int BOUTIQUES = 6;
        [CategoryInfo("Dessert", "Specialty Food & Drink")]
        public const int DESSERTS = 7;
        [CategoryInfo("Services", "Services")]
        public const int SERVICES = 8;
        [CategoryInfo("Marina", "Marina")]
        public const int MARINA = 9;

        public static CategoryInfo GetCategory(int category)
        {
            var fields = typeof(Categories).GetFields();
            foreach (var field in fields)
            {
                int fieldValue = (int)field.GetValue(typeof(Categories));
                if (fieldValue == category)
                {
                    CategoryInfo info = (CategoryInfo)Attribute.GetCustomAttribute(field, typeof(CategoryInfo));
                    return info;
                }
            }
            return new CategoryInfo("No Category", "No Category");
        }
    }
}