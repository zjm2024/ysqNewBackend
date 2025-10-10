using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessCard.Models
{
    // <summary>
    /// 问卷整体数据实体
    /// </summary>
    public class SurveyResponse
    {
        /// <summary>
        /// 问卷ID
        /// </summary>
        [JsonProperty("vid")]
        public int vid { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("begin_time")]
        public string BeginTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        [JsonProperty("atype")]
        public int Atype { get; set; }

        /// <summary>
        /// 问卷标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 问卷描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// 有效答案数
        /// </summary>
        [JsonProperty("answer_valid")]
        public int AnswerValid { get; set; }

        /// <summary>
        /// 总答案数
        /// </summary>
        [JsonProperty("answer_total")]
        public int AnswerTotal { get; set; }

        /// <summary>
        /// 状态（0-未发布，1-已发布等）
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// 审核状态（2-已审核等）
        /// </summary>
        [JsonProperty("verify_status")]
        public int VerifyStatus { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [JsonProperty("creater")]
        public string Creater { get; set; }

        /// <summary>
        /// 问题列表
        /// </summary>
        [JsonProperty("questions")]
        public List<Question> Questions { get; set; }

        /// <summary>
        /// 问题提取项（键为q_answerid）
        /// </summary>
        [JsonProperty("q_extractions")]
        public Dictionary<string, QExtraction> QExtractions { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        [JsonProperty("total_score")]
        public double TotalScore { get; set; }
    }

    /// <summary>
    /// 问题实体
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 是否甄别题
        /// </summary>
        [JsonProperty("is_zhenbie")]
        public bool IsZhenbie { get; set; }

        /// <summary>
        /// 最小时间（可能用于计时类问题）
        /// </summary>
        [JsonProperty("min_time")]
        public int MinTime { get; set; }

        /// <summary>
        /// 最大时间
        /// </summary>
        [JsonProperty("max_time")]
        public int MaxTime { get; set; }

        /// <summary>
        /// 问题序号
        /// </summary>
        [JsonProperty("q_index")]
        public int QIndex { get; set; }

        /// <summary>
        /// 问题类型（1-文本，3-单选，4-多选等）
        /// </summary>
        [JsonProperty("q_type")]
        public int QType { get; set; }

        /// <summary>
        /// 问题子类型
        /// </summary>
        [JsonProperty("q_subtype")]
        public int QSubtype { get; set; }

        /// <summary>
        /// 问题标题
        /// </summary>
        [JsonProperty("q_title")]
        public string QTitle { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [JsonProperty("is_requir")]
        public bool IsRequir { get; set; }

        /// <summary>
        /// 是否有跳转
        /// </summary>
        [JsonProperty("has_jump")]
        public bool HasJump { get; set; }

        /// <summary>
        /// 是否手动评分
        /// </summary>
        [JsonProperty("is_manual_score")]
        public bool? IsManualScore { get; set; } // 允许null

        /// <summary>
        /// 是否AI评分
        /// </summary>
        [JsonProperty("is_ai_grading")]
        public bool? IsAiGrading { get; set; } // 允许null

        /// <summary>
        /// 选项列表（单选/多选题才有）
        /// </summary>
        [JsonProperty("items")]
        public List<QuestionItem> Items { get; set; }

        /// <summary>
        /// 选项随机类型
        /// </summary>
        [JsonProperty("choice_randomtype")]
        public object ChoiceRandomtype { get; set; } // 暂用object兼容多种类型

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [JsonProperty("is_hide")]
        public bool? IsHide { get; set; } // 允许null
    }

    /// <summary>
    /// 问题选项实体
    /// </summary>
    public class QuestionItem
    {
        /// <summary>
        /// 选项图片（为空则无图片）
        /// </summary>
        [JsonProperty("item_image")]
        public string ItemImage { get; set; }

        /// <summary>
        /// 所属问题序号
        /// </summary>
        [JsonProperty("q_index")]
        public int QIndex { get; set; }

        /// <summary>
        /// 选项序号
        /// </summary>
        [JsonProperty("item_index")]
        public int ItemIndex { get; set; }

        /// <summary>
        /// 选项标题
        /// </summary>
        [JsonProperty("item_title")]
        public string ItemTitle { get; set; }

        /// <summary>
        /// 选项分值
        /// </summary>
        [JsonProperty("item_score")]
        public double ItemScore { get; set; }

        /// <summary>
        /// 是否被选中
        /// </summary>
        [JsonProperty("item_selected")]
        public bool ItemSelected { get; set; }
    }

    /// <summary>
    /// 问题提取项实体（q_extractions中的值）
    /// </summary>
    public class QExtraction
    {
        /// <summary>
        /// 问题类型
        /// </summary>
        [JsonProperty("q_type")]
        public int QType { get; set; }

        /// <summary>
        /// 问题子类型
        /// </summary>
        [JsonProperty("q_subtype")]
        public int QSubtype { get; set; }

        /// <summary>
        /// 选项列表（键为item_index）
        /// </summary>
        [JsonProperty("items")]
        public Dictionary<string, QuestionItem> Items { get; set; }

        /// <summary>
        /// 问题答案ID
        /// </summary>
        [JsonProperty("q_answerid")]
        public long QAnswerId { get; set; }

        /// <summary>
        /// 问题序号
        /// </summary>
        [JsonProperty("q_index")]
        public int QIndex { get; set; }
    }



}