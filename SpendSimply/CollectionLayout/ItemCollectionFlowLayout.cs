using System;
using CoreGraphics;
using Foundation;
using UIKit;
using CoreAnimation;

namespace SpendSimply
{
    public class LineLayout : UICollectionViewFlowLayout
    {
        //public const float ITEM_SIZE = 200.0f;
        //public const int ACTIVE_DISTANCE = 200;
        //public const float ZOOM_FACTOR = 0.3f;

        public const float ITEM_SIZE = 100.0f;
        public const int ACTIVE_DISTANCE = 20;
        public const float ZOOM_FACTOR = 0.3f;

        public LineLayout(nfloat width, nfloat height)
        {
            ItemSize = new CGSize(ITEM_SIZE, ITEM_SIZE);
            ScrollDirection = UICollectionViewScrollDirection.Vertical;
            SectionInset = new UIEdgeInsets(40, width - 40, 0, width - 40);
            MinimumLineSpacing = 40;
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            return true;
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var array = base.LayoutAttributesForElementsInRect(rect);
            var visibleRect = new CGRect(CollectionView.ContentOffset, CollectionView.Bounds.Size);

            foreach (var attributes in array)
            {
                if (attributes.Frame.IntersectsWith(rect))
                {
                    float distance = (float)(visibleRect.GetMidX() - attributes.Center.X);
                    //float distance = (float)(visibleRect.GetMidY() - attributes.Center.Y);
                    float normalizedDistance = distance / ACTIVE_DISTANCE;
                    if (Math.Abs(distance) < ACTIVE_DISTANCE)
                    {
                        float zoom = 1 + ZOOM_FACTOR * (1 - Math.Abs(normalizedDistance));
                        attributes.Transform3D = CATransform3D.MakeScale(zoom, zoom, 1.0f);
                        attributes.ZIndex = 1;
                    }
                }
            }
            return array;
        }

        public override CGPoint TargetContentOffset(CGPoint proposedContentOffset, CGPoint scrollingVelocity)
        {
            float offSetAdjustment = float.MaxValue;
            //float horizontalCenter = (float)(proposedContentOffset.X + (this.CollectionView.Bounds.Size.Width / 2.0));
            float verticalCenter = (float)(proposedContentOffset.Y + (this.CollectionView.Bounds.Size.Height / 2.0));
            CGRect targetRect = new CGRect(proposedContentOffset.Y, 0.0f, this.CollectionView.Bounds.Size.Height, this.CollectionView.Bounds.Size.Width);
            var array = base.LayoutAttributesForElementsInRect(targetRect);
            foreach (var layoutAttributes in array)
            {
                float itemHorizontalCenter = (float)layoutAttributes.Center.X;
                if (Math.Abs(itemHorizontalCenter - verticalCenter) < Math.Abs(offSetAdjustment))
                {
                    offSetAdjustment = itemHorizontalCenter - verticalCenter;
                }
            }
            return new CGPoint(proposedContentOffset.X + offSetAdjustment, proposedContentOffset.Y);
        }

    }
}