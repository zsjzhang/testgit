﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IQuestionnaireVisitorApp
    {
        bool GetAllQuestionnaireVisitor();

        int AddQuestionnaireVisitor(QuestionnaireVisitor entity);

        bool UpdateQuestionnaireVisitor(QuestionnaireVisitor entity);

        bool DeleteQuestionnaireVisitor(QuestionnaireVisitor entity);
    }
}